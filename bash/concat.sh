read -p "Enter script purpose: "  scriptpurp
read -p "Enter script: "  username
scriptpurp="# $scriptpurp"
printf "\n\n" >> ~/Dropbox/Projects/Scripts/bash/useful-scripts.sh
echo $scriptpurp >> ~/Dropbox/Projects/Scripts/bash/useful-scripts.sh
echo $username >> ~/Dropbox/Projects/Scripts/bash/useful-scripts.sh
