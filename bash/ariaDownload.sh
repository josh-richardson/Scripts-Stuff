#!/bin/bash

filename="$1"

while read -r line
do
    if [ "$line" ]
    then
        if [[ "$line" =~ (https?|ftp)\:\/\/ ]] 
        then
            echo "URL: '$line'"
            aria2c --file-allocation=none -c -x 10 -s 10 -d "$currdir" "$line"
        else 
            echo "Directory: '$line'"
            currdir="$line"
            if [ ! -d "$currdir" ]
            then
                mkdir -p "$currdir"
            fi
        fi
    fi
done < "$filename"
