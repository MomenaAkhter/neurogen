using System.Runtime.InteropServices;

namespace NeuroGen
{
    public class Database
    {
#if UNITY_IPHONE
   
       // On iOS plugins are statically linked into
       // the executable, so we have to use __Internal as the
       // library name.
       [DllImport ("__Internal")]

#else

        // Other platforms load plugins dynamically, so pass the name
        // of the plugin's dynamic library.
        [DllImport("Database")]

#endif
        public static extern int getSqliteVersion();

    }
}