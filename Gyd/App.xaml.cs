using System;
using System.Reflection;
using System.Windows;
using Gyd.Utils;

namespace Gyd
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static Assembly Assembly { get; } = Assembly.GetExecutingAssembly();
        public static string Company { get; } = Assembly.GetCompany();
        public static string Copyright { get; } = Assembly.GetCopyright();
        public static string Description { get; } = Assembly.GetDescription();
        public static string Directory { get; } = Assembly.GetDirectory();
        public static string Path { get; } = Assembly.GetPath();
        public static string Title { get; } = Assembly.GetTitle();
        public static Version Version { get; } = Assembly.GetVersion();
    }
}