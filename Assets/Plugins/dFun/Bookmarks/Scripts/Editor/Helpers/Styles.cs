using UnityEditor;
using UnityEngine;

namespace DFun.Bookmarks
{
    public static class Styles
    {
        public static GUIStyle BookmarkButtonStyleListView => EditorStyles.label;

        private static GUIStyle _bookmarkButtonStyleEditListView;
        public static GUIStyle BookmarkButtonStyleEditListView
        {
            get
            {
                if (_bookmarkButtonStyleEditListView == null)
                {
                    _bookmarkButtonStyleEditListView = new GUIStyle(EditorStyles.label)
                    {
                        fontSize = 14,
                        normal = new GUIStyleState
                        {
                            textColor = Color.yellow
                        },
                        hover = new GUIStyleState
                        {
                            textColor = Color.yellow
                        }
                    };
                }

                return _bookmarkButtonStyleEditListView;
            }
        }

        private static GUIStyle _bookmarkRemoveButtonStyleListView;
        public static GUIStyle BookmarkRemoveButtonStyleListView
        {
            get
            {
                if (_bookmarkRemoveButtonStyleListView == null)
                {
                    _bookmarkRemoveButtonStyleListView = new GUIStyle(EditorStyles.centeredGreyMiniLabel)
                    {
                        alignment = TextAnchor.LowerCenter
                    };
                }

                return _bookmarkRemoveButtonStyleListView;
            }
        }

        private static GUIStyle _bookmarkLabelStyleGridView;
        public static GUIStyle BookmarkLabelStyleGridView
        {
            get
            {
                if (_bookmarkLabelStyleGridView == null)
                {
                    // _bookmarkLabelStyleGridView = new GUIStyle("ProjectBrowserGridLabel")
                    _bookmarkLabelStyleGridView = new GUIStyle(EditorStyles.label)
                    {
                        alignment = TextAnchor.UpperCenter
                    };
                }

                return _bookmarkLabelStyleGridView;
            }
        }

        private static GUIStyle _bookmarkLabelStyleEditGridView;
        public static GUIStyle BookmarkLabelStyleEditGridView
        {
            get
            {
                if (_bookmarkLabelStyleEditGridView == null)
                {
                    _bookmarkLabelStyleEditGridView = new GUIStyle(BookmarkLabelStyleGridView)
                    {
                        fontSize = 14,
                        normal = new GUIStyleState
                        {
                            textColor = Color.yellow
                        }
                    };
                }

                return _bookmarkLabelStyleEditGridView;
            }
        }

        private static GUIStyle _bookmarkLabelStyleSelectedGridView;
        private static Texture2D _bookmarkLabelStyleSelectedGridViewTex;
        public static GUIStyle BookmarkLabelStyleSelectedGridView
        {
            get
            {
                if (_bookmarkLabelStyleSelectedGridView == null || _bookmarkLabelStyleSelectedGridViewTex == null)
                {
                    _bookmarkLabelStyleSelectedGridViewTex = CreateSelectionBgTexture();
                    GUIStyle baseStyle = BookmarkLabelStyleGridView;
                    _bookmarkLabelStyleSelectedGridView = new GUIStyle(baseStyle)
                    {
                        fontSize = 12,
                        normal = new GUIStyleState
                        {
                            textColor = baseStyle.normal.textColor,
                            background = _bookmarkLabelStyleSelectedGridViewTex
                        }
                    };
                }

                return _bookmarkLabelStyleSelectedGridView;
            }
        }

        public static GUIStyle BookmarkPreviewStyleGridView => "ObjectPickerResultsGrid";
        public static GUIStyle BookmarkPreviewStyleEditGridView => BookmarkPreviewStyleGridView;
        public static GUIStyle BookmarkPreviewStyleSelectedGridView => BookmarkPreviewStyleEditGridView;

        public static GUIStyle HelpButtonStyle => EditorStyles.label;

        private static GUIStyle _dragNDropLabelStyle;
        public static GUIStyle DragNDropLabelStyle
        {
            get
            {
                if (_dragNDropLabelStyle == null)
                {
                    _dragNDropLabelStyle = new GUIStyle(EditorStyles.wordWrappedLabel)
                    {
                        alignment = TextAnchor.UpperCenter,
                        fontSize = 14
                    };
                }

                return _dragNDropLabelStyle;
            }
        }

        public static GUIStyle TabHeaderStyle => EditorStyles.toolbarButton;

        private static GUIStyle _tabHeaderStyleActive;
        public static GUIStyle TabHeaderStyleActive
        {
            get
            {
                if (_tabHeaderStyleActive == null || _tabHeaderStyleActive.normal.background == null)
                {
                    _tabHeaderStyleActive = new GUIStyle(TabHeaderStyle);
                    Texture2D bgTexture = CreateTabHeaderStyleBgTexture();
                    _tabHeaderStyleActive.normal.background = bgTexture;
                    _tabHeaderStyleActive.normal.scaledBackgrounds = new[] { bgTexture };
                }

                return _tabHeaderStyleActive;
            }
        }

        private static GUIStyle _confirmationPopupLabel;
        public static GUIStyle ConfirmationPopupLabel
        {
            get
            {
                if (_confirmationPopupLabel == null)
                {
                    _confirmationPopupLabel = new GUIStyle(EditorStyles.label)
                    {
                        alignment = TextAnchor.MiddleCenter,
                        fontSize = 15,
                        normal = new GUIStyleState
                        {
                            textColor = new Color(255f / 255f, 105f / 255f, 45f / 255f, 1f)
                        }
                    };
                }

                return _confirmationPopupLabel;
            }
        }

        private static GUIStyle _lockButton;
        public static GUIStyle LockButton
        {
            get
            {
                if (_lockButton == null)
                {
                    _lockButton = "IN LockButton";
                }

                return _lockButton;
            }
        }

        private static Texture2D CreateTabHeaderStyleBgTexture()
        {
            Color bgColor = new Color(1.0f, 1.0f, 1.0f)
            {
                a = EditorGUIUtility.isProSkin ? 0.2f : 0.6f
            };
            return CreateStyleBgTexture(bgColor);
        }

        private static Texture2D CreateSelectionBgTexture()
        {
            Color bgColor = new Color(0.17f, 0.36f, 0.53f, 1f);
            return CreateStyleBgTexture(bgColor);
        }

        private static Texture2D CreateStyleBgTexture(Color color)
        {
            return MakeTex(256, 1, color);
        }

        /// https://forum.unity.com/threads/giving-unitygui-elements-a-background-color.20510/#post-422235
        public static Texture2D MakeTex(int width, int height, Color col)
        {
            Color[] pix = new Color[width * height];

            for (int i = 0; i < pix.Length; i++)
                pix[i] = col;

            Texture2D result = new Texture2D(width, height);
            result.SetPixels(pix);
            result.Apply();

            return result;
        }
    }
}