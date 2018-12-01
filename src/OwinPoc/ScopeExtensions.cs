using SimpleInjector;

namespace OwinPoc
{
    public static class ScopeExtensions
    {
        public static Scope AddInstance<T>(this Scope scope, T instance) where T : class
        {
            scope.SetItem(typeof(T).FullName, instance);
            return scope;
        }

        public static T Resolve<T>(this Scope scope) where T : class
        {
            if (scope.GetItem(typeof(T).FullName) is T item)
                return item;

            return scope.GetInstance<T>();
        }
    }
}