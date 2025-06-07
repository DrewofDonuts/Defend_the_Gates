namespace DFun.GameObjectResolver
{
    public class DefaultSceneObjectResolver : SceneObjectResolver
    {
        public static readonly SceneObjectResolver Instance = new DefaultSceneObjectResolver();

        public const string TypeName = "DFun.GameObjectResolver.DefaultSceneObjectResolver, DFun.SceneObjectResolver, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null";
        
        public DefaultSceneObjectResolver()
        {
            AddResolver(new DirectLinkResolver());
            AddResolver(new ActiveScenesByPathAndNameResolver());
        }
    }
}