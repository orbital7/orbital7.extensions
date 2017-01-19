using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;

namespace Orbital7.Extensions.Windows
{
    public static class ImpersonationHelper
    {
        [DllImport("advapi32.dll", CharSet = CharSet.Auto)]
        private static extern int LogonUser(
        string lpszUserName,
        String lpszDomain,
        String lpszPassword,
        int dwLogonType,
        int dwLogonProvider,
        ref IntPtr phToken);

        [DllImport("advapi32.dll",
         CharSet = System.Runtime.InteropServices.CharSet.Auto,
         SetLastError = true)]
        private extern static int DuplicateToken(
        IntPtr hToken,
        int impersonationLevel,
        ref IntPtr hNewToken);

        private static WindowsImpersonationContext m_impersonationContext;

        public static bool Impersonate(
         string user,
         string password,
         string domain)
        {
            WindowsIdentity tempWindowsIdentity;
            IntPtr token = IntPtr.Zero;
            IntPtr tokenDuplicate = IntPtr.Zero;

            // put ints here rather than consts to keep this line readable //
            if (LogonUser(user, domain, password, 9, 0, ref token) != 0)
            {
                if (DuplicateToken(token, 2, ref tokenDuplicate) != 0)
                {
                    tempWindowsIdentity = new WindowsIdentity(tokenDuplicate);
                    m_impersonationContext = tempWindowsIdentity.Impersonate();

                    if (null != m_impersonationContext)
                    {
                        return true;
                    }

                }
            }
            else
            {
                m_impersonationContext = null;
            }

            return false;
        }

        public static void UnImpersonate()
        {
            if (m_impersonationContext != null)
                m_impersonationContext.Undo();
        }
    }
}
