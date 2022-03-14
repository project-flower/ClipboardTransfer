ClipboardTransfer

概要:
  クリップボードを介してファイルを転送します。
  リモート デスクトップ等でファイルのコピーはできないが文字列のコピーができる場合に
  文字列を使用してファイルを転送することができます。

動作環境:
  Microsoft Windows 10
  .NET Framework 4.8

ファイル:
  ClipboardTransfer.exe
    実行ファイル
  ClipboardTransfer.exe.config
    設定ファイル

使い方:
  1. 転送元と転送先のデスクトップで ClipboardTransfer.exe を起動して下さい。

  2. File: に転送元は送信するファイル名、転送先は保存するファイル名を入力して下さい。
     空のままにすると、実行前にコモン ダイアログが開き、選択する事ができます。

  3. Timeout: に待ち時間のタイムアウト時間(秒)を指定して下さい。
     この時間を経過しても応答がない場合、転送が中断されます。

  4. Sending Buffer: に送信バッファ(バイトもしくは文字数)を入力して下さい。
     一度のクリップボードに転送するデータのサイズが指定されます。
     伝送モードがバイナリ(※)の場合はバイト数、
     伝送モードが文字列の場合はキャラクタ数を指定します。
     Windows のクリップボードは4MBが上限のため、4,194,304バイト以上の転送は失敗する可能性があります。
     (テキスト伝送モードの場合は1文字につき2バイトのため、最大2,097,152文字(サロゲートペア等含まず))

  5. Wait: に1回の転送でのウェイト(待機時間)をミリ秒で指定して下さい。
     ウェイトを小さくすると転送が速くなりますが、小さすぎると送信側と転送先の通信が失敗する場合があります。

  6. まず転送先で Receive をクリックします。
     File: が空の場合、ここで Save as ... のダイアログが開くので、
     保存するファイルを指定して OK をクリックして下さい。
     既に存在するファイルを指定した場合、ファイルの内容は上書きされます。

  7. "Please start sending from the sender within * seconds after clicking OK."
     と表示されたら、一旦そのままにします。

  8. 送信側で Send をクリックします。
     File: がからの場合、ここで Open のダイアログが開くので、
     送信するファイルを選択して OK をクリックして下さい。

  9. "Start reception on the receiving side. Click OK when it starts."
     と表示されたら、転送先で OK をクリックして、受信を開始し、
     送信側で OK をクリックして送信を開始して下さい。
     受信を開始する前に送信を開始すると転送が失敗します。
     転送が完了するまで、それぞれのデスクトップではクリップボードの操作を行わないで下さい。
     転送中にクリップボードが操作された場合、不正なデータが混入し、ファイルの内容が一致しなくなる場合があります。

  10. 転送中はタイトルバーに転送先には"Receiving..."、送信側には"Sending..."と表示されます。
      転送を途中でキャンセルする場合は、Cancel をクリックして下さい。

  11. 転送が完了すると、送信側には"Data transmission is complete. Check the MD5 value of the destination."と表示され、
      転送先には"Data reception is complete. Check the MD5 value."と表示されます。
      MD5: にそれぞれの MD5 値が表示されるので、転送元と転送先の値が同じであることを確認して下さい。
      一致していない場合、ファイルは正常に転送されていません。

  12. Show をクリックするとエクスプローラでファイルを表示します。

  ※バイナリ伝送モードは現在サポートされていません。

設定:
  設定ファイル ClipboardTransfer.exe.config の内容を変更することで、動作を変更できます。
  ・BufferSize
    Sending Buffer の初期値を設定します。
    設定可能値: 1 ～ 4194304
  ・InitialTimeout
    受信を開始してから最初のデータを待機する時間(ミリ秒)を設定します。
  ・RetryInterval
    クリップボード読み書きが失敗した時に再試行する間隔(ミリ秒)を設定します。
  ・RetryMax
    クリップボード読み書きが失敗した時に再試行する回数の上限を設定します。
  ・TimeoutSeconds
    Timeout: の初期値を設定します。
    設定可能値: 1 ～ 99999999
  ・TransmissionMode
    伝送モードを設定します。現在は"Strings"(文字列)しかサポートされません。
  ・TransmissionWait
    Wait: の初期値を設定します。
    設定可能値: 0 ～ 10000

    <configuration>
      <userSettings>
        <ClipboardTransfer.Properties.Settings>
          <setting name="BufferSize" serializeAs="String">
            <value>1048576</value>
          </setting>
          <setting name="InitialTimeout" serializeAs="String">
            <value>60000</value>
          </setting>
          <setting name="RetryInterval" serializeAs="String">
            <value>100</value>
          </setting>
          <setting name="RetryMax" serializeAs="String">
            <value>10</value>
          </setting>
          <setting name="TimeoutSeconds" serializeAs="String">
            <value>10</value>
          </setting>
          <setting name="TransmissionMode" serializeAs="String">
            <value>Strings</value>
          </setting>
          <setting name="TransmissionWait" serializeAs="String">
            <value>10</value>
          </setting>
        </ClipboardTransfer.Properties.Settings>
      </userSettings>
    </configuration>
