using Microsoft.Win32;

namespace AnignoLibrary.Misc.Registry
{
    public class RegistryWalker
    {
		#region (------  Events  ------)

        public event RegistryKeyFoundHandler RegistryKeyFound;

		#endregion (------  Events  ------)

		#region (------  Delegates  ------)

        public delegate void RegistryKeyFoundHandler(RegistryKey parent, string name, RegistryValueKind valueKind, object value);

		#endregion (------  Delegates  ------)

		#region (------  Public Methods  ------)

        public void Start(RegistryKey key)
        {
            string[] subkeys = key.GetSubKeyNames();
            foreach (string subkey in subkeys)
            {
                RegistryKey k = key.OpenSubKey(subkey);
                foreach (string s in k.GetValueNames())
                {
                    RaiseRegistryKeyFound(key, s, k.GetValueKind(s), k.GetValue(s));
                    Start(k);
                }
            }
        }

		#endregion (------  Public Methods  ------)

		#region (------  Private Methods  ------)

        private void RaiseRegistryKeyFound(RegistryKey parent, string name, RegistryValueKind valueKind, object value)
        {
            if (RegistryKeyFound != null)
            {
                RegistryKeyFound(parent, name, valueKind, value);
            }
        }

		#endregion (------  Private Methods  ------)
    }
}
