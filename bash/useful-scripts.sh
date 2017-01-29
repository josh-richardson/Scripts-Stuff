# Take screenshots with a 10 second interval on an android device 360 times... with delay this equates to roughly 1.5 hours:
for i in {1..360}; do adb shell "am start -a android.media.action.IMAGE_CAPTURE" && sleep 1 && adb shell "input keyevent 27" && sleep 10; done


# Un-archive all files beginning with part1.rar
for i in *part1.rar; do 7z x $i -y; done


# Insecure Git store credentials:
git config --global credential.helper store


# Counts lines of file types in a git repo:
git ls-files | grep '.py\|.html' | xargs cat | wc -l


# Listens on HTTP requests over a connection and returns a list of visited sites:
sudo ngrep -q -d wlp3s0 -W byline port 80 | grep Host