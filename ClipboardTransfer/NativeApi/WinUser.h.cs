using System;
using System.Runtime.InteropServices;

namespace NativeApi
{
    /// <summary>
    /// Predefined Clipboard Formats
    /// </summary>
    public static partial class CF
    {
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
    }

    public static partial class User32
    {
        /// <summary>
        /// クリップボードビューアのチェインから、指定されたウィンドウを削除します。
        /// </summary>
        /// <param name="hWndRemove">クリップボードビューアのチェインから削除したいウィンドウのハンドルを指定します。以前に SetClipboardViewer 関数に渡したハンドルでなければなりません。</param>
        /// <param name="hWndNewNext">クリップボードビューアのチェイン内で hWndRemove ウィンドウの次に存在するウィンドウのハンドルを指定します。このハンドルは、SetClipboardViewer 関数の戻り値です。ただし、 メッセージによりクリップボードビューアのチェインが変更された場合は、その限りではありません。クリップボードビューアのチェインが変更されるとこのメッセージが送信されるので、このメッセージを監視して、常に次のウィンドウを把握してください。</param>
        /// <returns>クリップボードビューアチェイン内のウィンドウに WM_CHANGECBCHAIN メッセージを渡した結果を示す値が返ります。クリップボードビューアチェイン内のウィンドウは、WM_CHANGECBCHAIN メッセージを処理すると、一般的には、0（FALSE）を返します。そのため、ChangeClipboardChain 関数の戻り値は、一般的には、0（FALSE）になります。クリップボードビューアチェイン内に、ウィンドウが 1 つしかなかったときの戻り値は、一般的に、0 以外の値（TRUE）になります。</returns>
        [DllImport(AssemblyName)]
        public static extern bool ChangeClipboardChain(IntPtr hWndRemove, IntPtr hWndNewNext);

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
        /// 1 つまたは複数のウィンドウへ、指定されたメッセージを送信します。この関数は、指定されたウィンドウのウィンドウプロシージャを呼び出し、そのウィンドウプロシージャがメッセージを処理し終わった後で、制御を返します。
        /// <para>メッセージを送信して即座に制御を返すには、SendMessageCallback または SendNotifyMessage 関数を使ってください。メッセージを 1 つのスレッドのメッセージキューにポストして即座に制御を返すには、PostMessage または PostThreadMessage 関数を使ってください。</para>
        /// </summary>
        /// <param name="hWnd">1 つのウィンドウのハンドルを指定します。このウィンドウのウィンドウプロシージャがメッセージを受信します。HWND_BROADCAST を指定すると、この関数は、システム内のすべてのトップレベルウィンドウ（親を持たないウィンドウ）へメッセージを送信します。無効になっている所有されていないウィンドウ、不可視の所有されていないウィンドウ、オーバーラップされた（手前にほかのウィンドウがあって覆い隠されている）ウィンドウ、ポップアップウィンドウも送信先になります。子ウィンドウへはメッセージを送信しません。</param>
        /// <param name="Msg">送信するべきメッセージを指定します。</param>
        /// <param name="wParam">メッセージ特有の追加情報を指定します。</param>
        /// <param name="lParam">メッセージ特有の追加情報を指定します。</param>
        /// <returns>メッセージ処理の結果が返ります。この戻り値の意味は、送信されたメッセージにより異なります。</returns>
        [DllImport(AssemblyName)]
        public static extern int PostMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

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

        /// <summary>
        /// クリップボードビューアのチェインに、指定されたウィンドウを追加します。クリップボードの内容が変更されると必ず、クリップボードビューアの各ウィンドウは WM_DRAWCLIPBOARD メッセージを受け取ります。
        /// </summary>
        /// <param name="hWndNewViewer">クリップボードのチェインに追加したいウィンドウのハンドルを指定します。</param>
        /// <returns>関数が成功すると、クリップボードビューアのチェイン内で、追加したウィンドウの次に位置するウィンドウのハンドルが返ります。エラーが発生した場合、または、クリップボードビューアのチェイン内に他のウィンドウが存在しなかった場合は、NULL が返ります。拡張エラー情報を取得するには、 関数を使います。</returns>
        [DllImport("user32")]
        public static extern IntPtr SetClipboardViewer(IntPtr hWndNewViewer);
    }

    public static partial class WM
    {
        public const int CUT = 0x0300;
        public const int COPY = 0x0301;
        public const int PASTE = 0x0302;
        public const int CLEAR = 0x0303;
        public const int UNDO = 0x0304;
        public const int RENDERFORMAT = 0x0305;
        public const int RENDERALLFORMATS = 0x0306;
        public const int DESTROYCLIPBOARD = 0x0307;
        public const int DRAWCLIPBOARD = 0x0308;
        public const int PAINTCLIPBOARD = 0x0309;
        public const int VSCROLLCLIPBOARD = 0x030A;
        public const int SIZECLIPBOARD = 0x030B;
        public const int ASKCBFORMATNAME = 0x030C;
        public const int CHANGECBCHAIN = 0x030D;
        public const int HSCROLLCLIPBOARD = 0x030E;
        public const int QUERYNEWPALETTE = 0x030F;
        public const int PALETTEISCHANGING = 0x0310;
        public const int PALETTECHANGED = 0x0311;
        public const int HOTKEY = 0x0312;
    }
}
