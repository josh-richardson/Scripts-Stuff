VarSetCapacity(APPBARDATA, A_PtrSize=4 ? 36:48)


; ---------------------
; -----KEY REMAPS------
; ---------------------
`::
Send \
Return

+`::
Send |
Return



; ---------------------
; -HIDE & SHOW TASKBAR-
; ---------------------

#v::
   NumPut(DllCall("Shell32\SHAppBarMessage", "UInt", 4 ; ABM_GETSTATE
                                           , "Ptr", &APPBARDATA
                                           , "Int")
 ? 2:1, APPBARDATA, A_PtrSize=4 ? 32:40) ; 2 - ABS_ALWAYSONTOP, 1 - ABS_AUTOHIDE
 , DllCall("Shell32\SHAppBarMessage", "UInt", 10 ; ABM_SETSTATE
                                    , "Ptr", &APPBARDATA)
   KeyWait, % A_ThisHotkey
   Return

   
   
   
; ---------------------
; --HOTKEYS FOR GAMES--
; ---------------------

$~^Z::
WinGetTitle, Title, A
 if(Title="Warcraft III") {
    Send {Enter}
    Send -worker
    Send {Enter}
 } else if(Title="Age of Mythology") {
    Send {Enter}
    Send ^V
    Send {Enter}
 }
 
 Return

 
; ---------------------
; --PROGRAM LAUNCHING--
; ---------------------
 
 
!^E::
path := gst()
Run, "C:\Program Files\paint.net\PaintDotNet.exe" "%path%"


gst() {   ; GetSelectedText or FilePath in Windows Explorer  by Learning one 

	IsClipEmpty := (Clipboard = "") ? 1 : 0

	if !IsClipEmpty {

		ClipboardBackup := ClipboardAll

		While !(Clipboard = "") {

			Clipboard =

			Sleep, 10

		}

	}

	Send, ^c

	ClipWait, 0.1

	ToReturn := Clipboard, Clipboard := ClipboardBackup

	if !IsClipEmpty

	ClipWait, 0.5, 1

	Return ToReturn

}