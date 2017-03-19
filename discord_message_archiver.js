// Discord message Saver V1.0: The disgusting ill-formatted hack created in 10 minutes.
// Scroll up to first message in the conversation and execute following javascript in the JS console, then scroll down to the end.
// When you've done so, type 'copy(messageDict);' into the console, and an array containing your messages will be copied to your clipboard.

var jq = document.createElement('script');
jq.src = "https://ajax.googleapis.com/ajax/libs/jquery/2.1.4/jquery.min.js";
document.getElementsByTagName('head')[0].appendChild(jq);

messageDict = [];
setTimeout(function() {
    setInterval(function(){ 
        $(".message-group").each(function(index) {
            var msgText = $(this).text();
            if (messageDict.indexOf(msgText) == -1) {
                messageDict.push(msgText);
            }
        });
    }, 500);
}, 1000);

// When finished: copy(messageDict);