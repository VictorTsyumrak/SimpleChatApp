//This file should be separated between view components but I will leave it for now as is.
const uri = "ws://" + window.location.host + "/ws" +"?groupId=" + groupId;

function connect() {
    const $connetion = new WebSocket(uri);
    $connetion.onopen = function (ev) {
        console.log('opened connection ' + uri);
    };

    $connetion.onclose = function (ev) {
        console.log('closed connection' + uri);
    };

    $connetion.onmessage = function (ev) {
        console.log('Length: ' + ev.data.length + ' ' + ev.data);
        
        let model = JSON.parse(ev.data);
        processChatMessage(model);
    };

    $connetion.onerror = function (ev) {
        console.log('error' + ev.data);
    };

    return $connetion;
}

const connection = connect();

$('#send-button').click(sendMessage);

$('#text-input').keyup(function (e) {
    if (e.keyCode !== 13) return;
    sendMessage();
});

function sendMessage(e) {
    let value = $('#text-input').val().trim();
    let name = $('#name-input').val().trim();
    if (!!value){
        connection.send(JSON.stringify({
            name: name,
            content: value
        }));
        $('#text-input').val('');
    }
}

function appendMessage(content) {
    let box = $('#message-box');
    box.append($('<div/>').text(content));
    box.scrollTop(box.prop('scrollHeight'));
}

function processChatMessage(model) {
    let message = `${model.AuthorName}: ${model.Content}`;
    appendMessage(message);
}
