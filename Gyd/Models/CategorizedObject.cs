namespace Gyd.Models
{
    public struct CategorizedObject
    {
        public CategorizedObject(string categoryName, object obj)
        {
            CategoryName = categoryName;
            Object = obj;
        }

        public string CategoryName { get; }
        public object Object { get; }
    }
}