using Microsoft.Win32;
using System;
using System.Security.AccessControl;
using System.Security.Principal;

namespace SpooferApp
{
    public class Peripherals
    {
        /// <summary>
        /// Blocks the registry access to the Registry Key Enum\USB.
        /// </summary>
        public static void BlockPeripherals()
        {
            // Open the USB registry key with read and write permissions
            using RegistryKey usbEnumKey = OpenUsbRegistryKeyWithReadWritePermissions();

                if (usbEnumKey == null)
            {
                throw new Exception(
                    "Unable to access the registry. Make sure you have administrative privileges."
                );
            }

            RegistrySecurity security = usbEnumKey.GetAccessControl();
            AuthorizationRuleCollection rules = security.GetAccessRules(
                true,
                true,
                typeof(SecurityIdentifier)
            );

            // Loop through each access rule and attempt to restrict permissions
            foreach (RegistryAccessRule rule in rules)
            {
                TryRestrictPermissionsForAccessRule(security, rule);
            }

            // Apply the new security settings to the key
            usbEnumKey.SetAccessControl(security);
        }

        private static RegistryKey OpenUsbRegistryKeyWithReadWritePermissions()
        {
            return Registry.LocalMachine.OpenSubKey(
                @"SYSTEM\CurrentControlSet\Enum\USB",
                RegistryKeyPermissionCheck.ReadWriteSubTree,
                RegistryRights.ChangePermissions
            );
        }

        private static void TryRestrictPermissionsForAccessRule(RegistrySecurity security, RegistryAccessRule rule)
        {
            SecurityIdentifier sid = (SecurityIdentifier)rule.IdentityReference;

            if (TryTranslateSecurityIdentifier(sid, out NTAccount account))
            {
                RegistryAccessRule newRule = new RegistryAccessRule(
                    account,
                    RegistryRights.ReadKey,
                    AccessControlType.Deny
                );

                // Try to add the new access rule to the security settings of the key
                try
                {
                    security.AddAccessRule(newRule);
                }
                catch (IdentityNotMappedException)
                {
                    // If the account associated with the new access rule cannot be mapped to a security identifier, continue to the next access rule
                }
            }
        }

        private static bool TryTranslateSecurityIdentifier(SecurityIdentifier sid, out NTAccount account)
        {
            try
            {
                account = (NTAccount)sid.Translate(typeof(NTAccount));
                return true;
            }
            catch (IdentityNotMappedException)
            {
                // If the security identifier cannot be translated, set the account to null and return false
                account = null;
                return false;
            }
        }
    }
}