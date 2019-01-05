# MiniAppManager Changelog

#### v.0.4.2 (2019-01) (WPF only)
- **New:** New update check with update window.
- **Changed:** Cleaned up `UpdateNotifyMode` (breaking).
- **Changed:** Moved `PortableSettingsProvider` to separate package.

#### v.0.4.1 (2018-10)
- **Fixed:** A few bugs in `PortableSettingsProvider`.
- **Fixed:** Saving/ Restoring of size for non-resizable windows.
- **Changed:** Moved more properties to `UpdateCheckEventArgs`.
- **Changed:** Custom settings now not roamed by default.

### v.0.4.0 (2018-07)
- **New:** Information for about box now all specified by assembly attributes.
- **New:** New readable settings format for custom settings properties.
- **Changed:** Extended `CheckForUpdatesCompleted` event.  
	(Now also raised if check for updates failed.)
- **Changed:** `UpdateNotifyMode` specifies showing of message box after update check.
- **Fixed:** Different serializations for custom settings properties.
- **Fixed:** For WPF: Little issues in about box and with display scaling.
- **Fixed:** Bug in `PortableSettingsProvider` (see #1).

#### v.0.3.1 (2018-04)
- **New:** Support for adding custom settings properties to be managed.
