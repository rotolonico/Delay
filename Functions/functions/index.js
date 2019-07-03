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

        if (numChildren > 1){            

            var firstChild;
            var secondChild;
    
            var index = 0;
            parentSnapshot.forEach(function(childSnapshot) {
                if (index === 0) firstChild = childSnapshot;    
                if (index === 1) secondChild = childSnapshot;
                index++;
            });            
    
            var gameId = secondChild.key + firstChild.key;
            console.log("Gameid: " + gameId)

            var mapName;
            if (Math.random() > 0.5){
                mapName = firstChild.val().map;
            } else {
                mapName = secondChild.val().map;
            }

            admin.database().ref("/maps").child(mapName).once("value")
            .then(function(mapSnapshot) {
                admin.database().ref("/games").child(gameId).update({
                    map: mapSnapshot.val(),
                    turn: firstChild.key,
                    [firstChild.key]: firstChild.val().nickname,
                    [secondChild.key]: secondChild.val().nickname
                })
        
                secondChild.ref.update({
                    gameid: gameId
                });
        
                firstChild.ref.update({
                    gameid: gameId
                });

                return null;
                
            }).catch(function(error) {
                console.log(error);
            });
        }

        return null;

    }).catch(function(error) {
        console.log(error);
    });

    return null;

});

