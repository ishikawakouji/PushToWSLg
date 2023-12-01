# PushToWSLg
Push some text to WSLg GUI text box

WSL, WSLg から起動するエディタなどにWindowsのIMEを利用して日本語入力が
できないので、暫定的に作りました。

入力したいGUIの入力したい場所にカーソルをあてておいてから
このプログラムのテキストボックスに文字を入力します。
そして、リンタンキーを押すごとに、テキストボックスの内容を
クリップボード経由で元のGUIのにペーストします。

This program put some text into another GUI even if the GUI
is WSLg GUI.

## Usage

1. start this program
1. start target GUI program
1. set cursor where you will put text
1. input any text in this program, you can use windows IME
1. when you press ENTER key, this program copy text to clipboard, hide own window, send Ctrl-V event to paste clipboard text

## TODO
- make it a resident program and start it with hotkey
- show window near target cursor position
- good looking window (^^)

