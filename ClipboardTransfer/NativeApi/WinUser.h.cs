using System;
using System.Runtime.InteropServices;

namespace NativeApi
{
    /// <summary>
    /// Predefined Clipboard Formats
    /// </summary>
    public static partial class CF
    {
        #region Public Fields

        public const uint TEXT = 1;
        public const uint BITMAP = 2;
        public const uint METAFILEPICT = 3;
        public const uint SYLK = 4;
        public const uint DIF = 5;
        public const uint TIFF = 6;
        public const uint OEMTEXT = 7;
        public const uint DIB = 8;
        public const uint PALETTE = 9;
        public const uint PENDATA = 10;
        public const uint RIFF = 11;
        public const uint WAVE = 12;
        public const uint UNICODETEXT = 13;
        public const uint ENHMETAFILE = 14;
        public const uint HDROP = 15;
        public const uint LOCALE = 16;
        public const uint DIBV5 = 17;
        public const uint MAX = 18;

        #endregion
    }

    public static partial class User32
    {
        #region Public Methods

        /// <summary>
        /// Places the given window in the system-maintained clipboard format listener list.
        /// </summary>
        /// <param name="hwnd">A handle to the window to be placed in the clipboard format listener list.</param>
        /// <returns>Returns TRUE if successful, FALSE otherwise. Call GetLastError for additional details.</returns>
        [DllImport(AssemblyName, SetLastError = true)]
        public static extern bool AddClipboardFormatListener(IntPtr hwnd);

        /// <summary>
        /// Closes the clipboard.
        /// </summary>
        /// <returns>
        /// If the function succeeds, the return value is nonzero.
        /// If the function fails, the return value is zero.To get extended error information, call GetLastError.
        /// </returns>
        [DllImport(AssemblyName, SetLastError = true)]
        public static extern bool CloseClipboard();

        /// <summary>
        /// Empties the clipboard and frees handles to data in the clipboard. The function then assigns ownership of the clipboard to the window that currently has the clipboard open.
        /// </summary>
        /// <returns>
        /// If the function succeeds, the return value is nonzero.
        /// If the function fails, the return value is zero.To get extended error information, call GetLastError.
        /// </returns>
        [DllImport(AssemblyName, SetLastError = true)]
        public static extern bool EmptyClipboard();

        /// <summary>
        /// Opens the clipboard for examination and prevents other applications from modifying the clipboard content.
        /// </summary>
        /// <param name="hWndNewOwner">A handle to the window to be associated with the open clipboard. If this parameter is NULL, the open clipboard is associated with the current task.</param>
        /// <returns>
        /// If the function succeeds, the return value is nonzero.
        /// If the function fails, the return value is zero.To get extended error information, call GetLastError.
        /// </returns>
        [DllImport(AssemblyName, SetLastError = true)]
        public static extern bool OpenClipboard(IntPtr hWndNewOwner);

        /// <summary>
        /// Removes the given window from the system-maintained clipboard format listener list.
        /// </summary>
        /// <param name="hwnd">A handle to the window to remove from the clipboard format listener list.</param>
        /// <returns>Returns TRUE if successful, FALSE otherwise. Call GetLastError for additional details.</returns>
        [DllImport(AssemblyName, SetLastError = true)]
        public static extern bool RemoveClipboardFormatListener(IntPtr hwnd);

        /// <summary>
        /// Places data on the clipboard in a specified clipboard format. The window must be the current clipboard owner, and the application must have called the OpenClipboard function. (When responding to the WM_RENDERFORMAT message, the clipboard owner must not call OpenClipboard before calling SetClipboardData.)
        /// </summary>
        /// <param name="uFormat">The clipboard format. This parameter can be a registered format or any of the standard clipboard formats. For more information, see Standard Clipboard Formats and Registered Clipboard Formats.</param>
        /// <param name="hMem">
        /// A handle to the data in the specified format. This parameter can be NULL, indicating that the window provides data in the specified clipboard format (renders the format) upon request; this is known as delayed rendering. If a window delays rendering, it must process the WM_RENDERFORMAT and WM_RENDERALLFORMATS messages.
        /// <para>If SetClipboardData succeeds, the system owns the object identified by the hMem parameter. The application may not write to or free the data once ownership has been transferred to the system, but it can lock and read from the data until the CloseClipboard function is called. (The memory must be unlocked before the Clipboard is closed.) If the hMem parameter identifies a memory object, the object must have been allocated using the function with the GMEM_MOVEABLE flag.</para>
        /// </param>
        /// <returns>
        /// If the function succeeds, the return value is the handle to the data.
        /// If the function fails, the return value is NULL.To get extended error information, call GetLastError.
        /// </returns>
        [DllImport(AssemblyName, SetLastError = true)]
        public static extern IntPtr SetClipboardData(uint uFormat, IntPtr hMem);

        #endregion
    }

    public static partial class WM
    {
        #region Public Fields

        public const int CLIPBOARDUPDATE = 0x031D;

        #endregion
    }
}
