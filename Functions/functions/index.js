const functions = require('firebase-functions');

const admin = require('firebase-admin');
admin.initializeApp();

exports.onNewPlayer = functions.database
.ref('/waitingroom/{playerId}')
.onCreate((snapshot, context) => {
    var parent = admin.database().ref("/waitingroom");
    parent.once("value")
    .then(function(parentSnapshot) {

        var numChildren = parentSnapshot.numChildren();

        if (numChildren % 2 === 0){            

            var lastChild;
            var secondLastChild;
    
            parentSnapshot.forEach(function(childSnapshot) {    
                lastChild = childSnapshot;
            });
            parentSnapshot.forEach(function(childSnapshot) {    
                if (childSnapshot.key !== lastChild.key) secondLastChild = childSnapshot;
            });
    
            var gameId = lastChild.key + secondLastChild.key;
            console.log("Gameid: " + gameId)

            var map;
            if (Math.random() > 0.5){
                map = secondLastChild.val().map;
            } else {
                map = lastChild.val().map;
            }
            
            admin.database().ref("/games").child(gameId).update({
                map: map,
                turn: secondLastChild.key
            })
    
            lastChild.ref.update({
                gameid: gameId
            });
    
            secondLastChild.ref.update({
                gameid: gameId
            });
        }

        return null;

    }).catch(function(error) {
        console.log(error);
    });

    return null;

});

