using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using RayFire;

namespace RayFireEditor
{
    [CanEditMultipleObjects]
    [CustomEditor (typeof(RayfireMan))]
    public class RayfireManEditor : Editor
    {
        RayfireMan man;
        Texture2D  logo;
        Texture2D  icon;
        
        // Foldout
        static bool fld_adv;
        static bool fld_mat;
        static bool fld_mat_heavy;
        static bool fld_mat_light;
        static bool fld_mat_dense;
        static bool fld_mat_porous;
        static bool fld_mat_concrete;
        static bool fld_mat_bricks;
        static bool fld_mat_glass;
        static bool fld_mat_rubber;
        static bool fld_mat_ice;
        static bool fld_mat_wood;

        // Minimum & Maximum ranges
        const float multiplier_min     = 0;
        const float multiplier_max     = 1f;
        const float collider_size_min  = 0;
        const float collider_size_max  = 1f;
        const int   coplanar_verts_min = 0;
        const int   coplanar_verts_max = 999;
        const float minimum_mass_min   = 0.001f;
        const float minimum_mass_max   = 1f;
        const float maximum_mass_min   = 0.1f;
        const float maximum_mass_max   = 4000f;
        const float solidity_min       = 0f;
        const float solidity_max       = 5f;
        const float quota_min          = 0f;
        const float quota_max          = 0.1f;
        const int   bad_min            = 1;
        const int   bad_max            = 10;
        const float shadow_min         = 0;
        const float shadow_max         = 1f;
        const int   frag_cap_min       = 0;
        const int   frag_cap_max       = 10000;

        // Serialized properties
        SerializedProperty sp_phy_set;
        SerializedProperty sp_phy_int;
        SerializedProperty sp_phy_mul;
        SerializedProperty sp_phy_col;
        SerializedProperty sp_phy_cop;
        SerializedProperty sp_phy_cok;
        SerializedProperty sp_col_mesh;
        SerializedProperty sp_col_cls;
        SerializedProperty sp_mat_min;
        SerializedProperty sp_mat_max;
        
        /*
        SerializedProperty sp_mat_pres;
        SerializedProperty sp_mat_dest;
        SerializedProperty sp_mat_sol;
        SerializedProperty sp_mat_dens;
        SerializedProperty sp_mat_drag;
        SerializedProperty sp_mat_ang;
        SerializedProperty sp_mat_mat;
        SerializedProperty sp_mat_dyn;
        SerializedProperty sp_mat_stat;
        SerializedProperty sp_mat_bnc;
        */

        SerializedProperty sp_act_par;
        SerializedProperty sp_dml_sol;
        SerializedProperty sp_dml_time;
        SerializedProperty sp_adv_expand;
        SerializedProperty sp_adv_parent;
        SerializedProperty sp_adv_global;
        SerializedProperty sp_adv_current;
        SerializedProperty sp_adv_amount;
        SerializedProperty sp_adv_bad;
        SerializedProperty sp_adv_size;
        SerializedProperty sp_pol_frg;
        SerializedProperty sp_pol_prt;
        SerializedProperty sp_pol_reu;
        SerializedProperty sp_pol_min;
        SerializedProperty sp_pol_max;
        SerializedProperty sp_dbg_msg;
        SerializedProperty sp_dbg_bld;
        
        private void OnEnable()
        {
            // Get component
            man = (RayfireMan)target;
            
            // Find properties
            sp_phy_set  = serializedObject.FindProperty(nameof(man.setGravity));
            sp_phy_mul  = serializedObject.FindProperty(nameof(man.multiplier));
            sp_phy_int  = serializedObject.FindProperty(nameof(man.interpolation));
            sp_phy_col  = serializedObject.FindProperty(nameof(man.colliderSize));
            sp_phy_cop  = serializedObject.FindProperty(nameof(man.coplanarVerts));
            sp_phy_cok  = serializedObject.FindProperty(nameof(man.cookingOptions));
            sp_col_mesh = serializedObject.FindProperty(nameof(man.meshCollision));
            sp_col_cls  = serializedObject.FindProperty(nameof(man.clusterCollision));
            sp_mat_min  = serializedObject.FindProperty(nameof(man.minimumMass));
            sp_mat_max  = serializedObject.FindProperty(nameof(man.maximumMass));
            
            /*
            https://docs.unity3d.com/ScriptReference/SerializedObject.html
            sp_mat_pres    = serializedObject.FindProperty(nameof(man.materialPresets));
            sp_mat_dest    = serializedObject.FindProperty(nameof(man.setGravity));
            sp_mat_sol     = serializedObject.FindProperty(nameof(man.setGravity));
            sp_mat_dens    = serializedObject.FindProperty(nameof(man.setGravity));
            sp_mat_drag    = serializedObject.FindProperty(nameof(man.setGravity));
            sp_mat_ang     = serializedObject.FindProperty(nameof(man.setGravity));
            sp_mat_mat     = serializedObject.FindProperty(nameof(man.setGravity));
            sp_mat_dyn     = serializedObject.FindProperty(nameof(man.setGravity));
            sp_mat_stat    = serializedObject.FindProperty(nameof(man.setGravity));
            sp_mat_bnc     = serializedObject.FindProperty(nameof(man.setGravity));
            */
            
            sp_act_par     = serializedObject.FindProperty(nameof(man.parent));
            sp_dml_sol     = serializedObject.FindProperty(nameof(man.globalSolidity));
            sp_dml_time    = serializedObject.FindProperty(nameof(man.timeQuota));
            sp_adv_expand  = serializedObject.FindProperty(nameof(man.advancedDemolitionProperties));
            sp_adv_parent  = serializedObject.FindProperty(nameof(man.advancedDemolitionProperties) + "." + nameof(man.advancedDemolitionProperties.parent));
            sp_adv_global  = serializedObject.FindProperty(nameof(man.advancedDemolitionProperties) + "." + nameof(man.advancedDemolitionProperties.globalParent));
            sp_adv_current = serializedObject.FindProperty(nameof(man.advancedDemolitionProperties) + "." + nameof(man.advancedDemolitionProperties.currentAmount));
            sp_adv_amount  = serializedObject.FindProperty(nameof(man.advancedDemolitionProperties) + "." + nameof(man.advancedDemolitionProperties.maximumAmount));
            sp_adv_bad     = serializedObject.FindProperty(nameof(man.advancedDemolitionProperties) + "." + nameof(man.advancedDemolitionProperties.badMeshTry));
            sp_adv_size    = serializedObject.FindProperty(nameof(man.advancedDemolitionProperties) + "." + nameof(man.advancedDemolitionProperties.sizeThreshold));
            sp_pol_frg     = serializedObject.FindProperty(nameof(man.fragments) + "." + nameof(man.fragments.enable));
            sp_pol_reu     = serializedObject.FindProperty(nameof(man.fragments) + "." + nameof(man.fragments.reuse));
            sp_pol_min     = serializedObject.FindProperty(nameof(man.fragments) + "." + nameof(man.fragments.minCap));
            sp_pol_max     = serializedObject.FindProperty(nameof(man.fragments) + "." + nameof(man.fragments.maxCap));
            sp_pol_prt     = serializedObject.FindProperty(nameof(man.particles) + "." + nameof(man.particles.enable));
            sp_dbg_msg     = serializedObject.FindProperty(nameof(man.debug));
            sp_dbg_bld     = serializedObject.FindProperty(nameof(man.debugBuild));
            
            // Foldouts
            if (EditorPrefs.HasKey (TextKeys.man_fld_adv) == true) fld_adv = EditorPrefs.GetBool (TextKeys.man_fld_adv);
        }
        
        /// /////////////////////////////////////////////////////////
        /// Inspector
        /// /////////////////////////////////////////////////////////
        
        public override void OnInspectorGUI()
        {
            // Update changed properties
            serializedObject.Update();
            
            // Set new static instance
            if (RayfireMan.inst == null)
                RayfireMan.inst = man;
            
            if (Application.isPlaying == true)
            {
                if (GUILayout.Button (TextMan.gui_btn_dest_frags, GUILayout.Height (20)))
                    RayfireMan.inst.storage.DestroyAll();
                
                GUICommon.Space ();
            }
            
            GUI_Physics();
            GUI_Collision();
            GUI_Materials();
            GUI_Activation();
            UI_Demolition();
            UI_Pooling();
            UI_Info();
            UI_About();
            
            // Apply changes
            serializedObject.ApplyModifiedProperties();
        }

        /// /////////////////////////////////////////////////////////
        /// Physics
        /// /////////////////////////////////////////////////////////
        
        void GUI_Physics()
        {
            GUICommon.Caption (TextMan.gui_cap_phy);
            GUICommon.PropertyField (sp_phy_set, TextMan.gui_phy_set);
            if (man.setGravity == true)
                GUICommon.Slider (sp_phy_mul, multiplier_min, multiplier_max, TextMan.gui_phy_mul);
            GUICommon.PropertyField (sp_phy_int, TextMan.gui_phy_int);

            GUICommon.Caption (TextMan.gui_cap_col);
            GUICommon.Slider (sp_phy_col, collider_size_min,  collider_size_max,  TextMan.gui_phy_col);
            GUICommon.IntSlider (sp_phy_cop, coplanar_verts_min, coplanar_verts_max, TextMan.gui_phy_cop);
            GUICommon.PropertyField (sp_phy_cok, TextMan.gui_phy_cok);
        }

        /// /////////////////////////////////////////////////////////
        /// Collision
        /// /////////////////////////////////////////////////////////
        
        void GUI_Collision()
        {
            GUICommon.Caption (TextMan.gui_cap_det);
            GUICommon.PropertyField (sp_col_mesh, TextMan.gui_col_mesh);
            GUICommon.PropertyField (sp_col_cls,  TextMan.gui_col_cls);
        }

        /// /////////////////////////////////////////////////////////
        /// Materials
        /// /////////////////////////////////////////////////////////
        
        void GUI_Materials()
        {
            GUICommon.Caption (TextMan.gui_cap_mat);
            GUICommon.Slider (sp_mat_min, minimum_mass_min, minimum_mass_max, TextMan.gui_mat_min);
            GUICommon.Slider (sp_mat_max, maximum_mass_min, maximum_mass_max, TextMan.gui_mat_max);
            
            UI_Materials_Presets();
        }

        void UI_Materials_Presets()
        {
            fld_mat = EditorGUILayout.Foldout (fld_mat, TextMan.gui_mat_pres, true);
            if (fld_mat == true)
            {
                EditorGUI.indentLevel++;
                UI_Material (man.materialPresets.heavyMetal, ref fld_mat_heavy, "Heavy Metal");
                UI_Material (man.materialPresets.lightMetal, ref fld_mat_light, "Light Metal");
                UI_Material (man.materialPresets.denseRock,  ref fld_mat_dense,  "Dense Rock");
                UI_Material (man.materialPresets.porousRock, ref fld_mat_porous, "Porous Rock");
                UI_Material (man.materialPresets.concrete,   ref fld_mat_concrete,   "Concrete");
                UI_Material (man.materialPresets.brick,      ref fld_mat_bricks,      "Brick");
                UI_Material (man.materialPresets.glass,      ref fld_mat_glass,      "Glass");
                UI_Material (man.materialPresets.rubber,     ref fld_mat_rubber,     "Rubber");
                UI_Material (man.materialPresets.ice,        ref fld_mat_ice,        "Ice");
                UI_Material (man.materialPresets.wood,       ref fld_mat_wood,       "Wood");
                EditorGUI.indentLevel--;
            }
        }

        void UI_Material(RFMaterial mat, ref bool state, string cap)
        {
            state = EditorGUILayout.Foldout (state, cap, true);
            if (state == true)
            {
                GUICommon.Space ();
                
                EditorGUI.indentLevel++;
                
                GUILayout.Label ("          Demolition", EditorStyles.boldLabel);
                
                EditorGUI.BeginChangeCheck();
                bool destructible = EditorGUILayout.Toggle (TextMan.gui_mat_dest, mat.destructible);
                if (EditorGUI.EndChangeCheck() == true)
                {
                    Undo.RecordObjects (targets, TextMan.gui_mat_dest.text);
                    mat.destructible = destructible;
                    SetDirty (man);
                }
                
                GUICommon.Space ();
                
                EditorGUI.BeginChangeCheck();
                int solidity = EditorGUILayout.IntSlider (TextMan.gui_mat_sol, mat.solidity, 0, 100);
                if (EditorGUI.EndChangeCheck() == true)
                {
                    Undo.RecordObjects (targets, TextMan.gui_mat_sol.text);
                    mat.solidity = solidity;
                    SetDirty (man);
                }
                
                GUILayout.Label ("          Rigid Body", EditorStyles.boldLabel);
                
                EditorGUI.BeginChangeCheck();
                float density = EditorGUILayout.Slider (TextMan.gui_mat_dens, mat.density, 0.01f, 100f);
                if (EditorGUI.EndChangeCheck() == true)
                {
                    Undo.RecordObjects (targets, TextMan.gui_mat_dens.text);
                    mat.density = density;
                    SetDirty (man);
                }
                
                GUICommon.Space ();
                
                EditorGUI.BeginChangeCheck();
                float drag = EditorGUILayout.Slider (TextMan.gui_mat_drag, mat.drag, 0f, 1f);
                if (EditorGUI.EndChangeCheck() == true)
                {
                    Undo.RecordObjects (targets, TextMan.gui_mat_drag.text);
                    mat.drag = drag;
                    SetDirty (man);
                }
                
                GUICommon.Space ();
                
                EditorGUI.BeginChangeCheck();
                float angularDrag = EditorGUILayout.Slider (TextMan.gui_mat_ang, mat.angularDrag, 0f, 1f);
                if (EditorGUI.EndChangeCheck() == true)
                {
                    Undo.RecordObjects (targets, TextMan.gui_mat_ang.text);
                    mat.angularDrag = angularDrag;
                    SetDirty (man);
                }
                
                GUILayout.Label ("          Physic Material", EditorStyles.boldLabel);
                
                GUICommon.Space ();
                
                EditorGUI.BeginChangeCheck();
                PhysicsMaterial material = (PhysicsMaterial)EditorGUILayout.ObjectField (TextMan.gui_mat_mat, mat.material, typeof(PhysicsMaterial), true);
                if (EditorGUI.EndChangeCheck() == true)
                {
                    Undo.RecordObjects (targets, TextMan.gui_mat_mat.text);
                    mat.material = material;
                    SetDirty (man);
                }
                
                EditorGUI.BeginChangeCheck();
                float dynamicFriction = EditorGUILayout.Slider (TextMan.gui_mat_dyn, mat.dynamicFriction, 0.01f, 1f);
                if (EditorGUI.EndChangeCheck() == true)
                {
                    Undo.RecordObjects (targets, TextMan.gui_mat_dyn.text);
                    mat.dynamicFriction = dynamicFriction;
                    SetDirty (man);
                }
                
                GUICommon.Space ();
                
                EditorGUI.BeginChangeCheck();
                float staticFriction = EditorGUILayout.Slider (TextMan.gui_mat_stat, mat.staticFriction, 0.01f, 1f);
                if (EditorGUI.EndChangeCheck() == true)
                {
                    Undo.RecordObjects (targets, TextMan.gui_mat_stat.text);
                    mat.staticFriction = staticFriction;
                    SetDirty (man);
                }
                
                GUICommon.Space ();
                
                EditorGUI.BeginChangeCheck();
                float bounciness = EditorGUILayout.Slider (TextMan.gui_mat_bnc, mat.bounciness, 0.01f, 1f);
                if (EditorGUI.EndChangeCheck() == true)
                {
                    Undo.RecordObjects (targets, TextMan.gui_mat_bnc.text);
                    mat.bounciness = bounciness;
                    SetDirty (man);
                }
                
                EditorGUI.indentLevel--;
            }
            
            GUICommon.Space ();
        }
        
        /// /////////////////////////////////////////////////////////
        /// Activation
        /// /////////////////////////////////////////////////////////

        void GUI_Activation()
        {
            GUICommon.Caption (TextMan.gui_cap_axt);
            GUICommon.PropertyField (sp_act_par, TextMan.gui_act_par);
        }

        /// /////////////////////////////////////////////////////////
        /// Demolition
        /// /////////////////////////////////////////////////////////
        
        void UI_Demolition()
        {
            GUICommon.Caption (TextMan.gui_cap_dml);
            GUICommon.Slider (sp_dml_sol,  solidity_min, solidity_max, TextMan.gui_dml_sol);
            GUICommon.Slider (sp_dml_time, quota_min,    quota_max, TextMan.gui_dml_time);
            
            UI_Demolition_Adv();
        }

        void UI_Demolition_Adv()
        {
            GUICommon.Foldout (ref fld_adv, TextKeys.man_fld_adv, TextMan.gui_adv_expand.text);
            if (fld_adv == true)
            {
                EditorGUI.indentLevel++;

                GUICommon.Caption (TextMan.gui_cap_frg);
                GUICommon.PropertyField (sp_adv_parent, TextMan.gui_adv_parent);
                if (man.advancedDemolitionProperties.parent == RFManDemolition.FragmentParentType.GlobalParent)
                    GUICommon.PropertyField (sp_adv_global, TextMan.gui_adv_global);
                GUICommon.PropertyField (sp_adv_current, TextMan.gui_adv_current);
                GUICommon.PropertyField (sp_adv_amount,  TextMan.gui_adv_amount);
                GUICommon.IntSlider (sp_adv_bad, bad_min, bad_max, TextMan.gui_adv_bad);
   
                GUICommon.Caption (TextMan.gui_cap_shad);
                GUICommon.Slider (sp_adv_size, shadow_min, shadow_max, TextMan.gui_adv_size);

                EditorGUI.indentLevel--;
            }
        }

        /// /////////////////////////////////////////////////////////
        /// Pooling
        /// /////////////////////////////////////////////////////////
        
        void UI_Pooling()
        {
            GUICommon.Caption (TextMan.gui_cap_pol);
            GUICommon.PropertyField (sp_pol_frg, TextMan.gui_pol_frg);
            if (man.fragments.enable == true)
            {
                GUICommon.PropertyField (sp_pol_reu, TextMan.gui_pol_reu);
                GUICommon.IntSlider (sp_pol_min, frag_cap_min, frag_cap_max, TextMan.gui_pol_min);
                if (man.fragments.reuse == true)
                    GUICommon.IntSlider (sp_pol_max, frag_cap_min, frag_cap_max, TextMan.gui_pol_max);
            }

            EditorGUI.BeginChangeCheck();
            GUICommon.PropertyField (sp_pol_prt, TextMan.gui_pol_prt);
            if (EditorGUI.EndChangeCheck() == true)
                man.particles.Enable = man.particles.enable;
            
            // Info
            if (man.particles.enable == true)
            {
                if (man.particles.emitters != null && man.particles.emitters.Count > 0)
                {
                    GUICommon.Space ();
                    GUILayout.Label (TextMan.str_prt_emit + man.particles.emitters.Count,        EditorStyles.boldLabel);
                    GUICommon.Space ();
                    GUILayout.Label (TextMan.str_prt_amount+ man.particles.GetTotalPoolAmount(), EditorStyles.boldLabel);
                    GUICommon.Space ();
                    GUILayout.Label (TextMan.str_prt_reu + man.particles.reused,                  EditorStyles.boldLabel);
                    GUICommon.Space ();
                    GUILayout.Label (TextMan.str_prt_scene + man.particles.ResetCheck(),          EditorStyles.boldLabel);
                }
            }
        }

        /// /////////////////////////////////////////////////////////
        /// Info
        /// /////////////////////////////////////////////////////////

        void UI_Info()
        {
            if (Application.isPlaying == true)
            {
                GUICommon.Space ();
                GUILayout.Label (TextMan.str_info, EditorStyles.boldLabel);
                GUICommon.Space ();
                
                if (man.fragments.enable == true && man.fragments.queue.Count > 0)
                    GUILayout.Label (TextMan.str_rigs+ man.fragments.queue.Count);
                
                GUICommon.Space ();
                
                if (man.advancedDemolitionProperties.currentAmount > 0)
                    GUILayout.Label (TextMan.str_frags+ man.advancedDemolitionProperties.currentAmount + "/" + man.advancedDemolitionProperties.maximumAmount);

                GUICommon.Space ();

                if (man.physicList != null && man.physicList.Count > 0)
                    GUILayout.Label (TextMan.str_vel+ man.physicList.Count);
            }
        }

        /// /////////////////////////////////////////////////////////
        /// About
        /// /////////////////////////////////////////////////////////
        
        void UI_About()
        {
            GUICommon.Caption (TextMan.gui_cap_abt);
            GUICommon.PropertyField (sp_dbg_msg, TextMan.gui_dbg_msg);
            if (man.debug == true)
                GUICommon.PropertyField (sp_dbg_bld, TextMan.gui_dbg_bld);
            
            GUICommon.Space ();
            GUILayout.Label (TextMan.str_build + RayfireMan.buildMajor + '.' + RayfireMan.buildMinor.ToString ("D2"));
            GUICommon.Space ();

            // Logo TODO remove if component removed
            if (logo == null)
                logo = (Texture2D)AssetDatabase.LoadAssetAtPath ("Assets/RayFire/Info/Logo/logo_small.png", typeof(Texture2D));
            if (logo != null)
                GUILayout.Box (logo, GUILayout.Width ((int)EditorGUIUtility.currentViewWidth - 19f), GUILayout.Height (64));
            
            if (GUILayout.Button (TextMan.gui_change, GUILayout.Height (20)))
                Application.OpenURL (TextMan.str_url);
        }

        /// /////////////////////////////////////////////////////////
        /// Common
        /// /////////////////////////////////////////////////////////

        void SetDirty (RayfireMan scr)
        {
            if (Application.isPlaying == false)
            {
                EditorUtility.SetDirty (scr);
                EditorSceneManager.MarkSceneDirty (scr.gameObject.scene);
            }
        }
    }
}