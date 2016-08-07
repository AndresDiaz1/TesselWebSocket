    // HTTP Request routing library
var router = require('tiny-router'),
    // Websocket library
    ws = require("nodejs-websocket"),
    // Use fs for static files
    fs = require('fs'),
    // Use tessel for changing the LEDs
    tessel = require('tessel');

    var analogReadPin = tessel.port.B.pin[7];
 
// The router should use our static folder for client HTML/JS
router
    .use('static', {path: './static'})
    // Use the onboard file system (as opposed to microsd)
    .use('fs', fs)

        .get('/', function(req, res) {
 var body = ['<!DOCTYPE html>',
        '<html ng-app="tessel">',
           '<head>',
            '<title>Camera Module</title>',
            '<link href="//maxcdn.bootstrapcdn.com/bootstrap/3.2.0/css/bootstrap.min.css" rel="stylesheet"/>',
            '<script src="https://ajax.googleapis.com/ajax/libs/angularjs/1.3.0-beta.17/angular.min.js"></script>',
           '</head>',
        '<body style="background-color:#222;">',
            '<div class="col-md-6 col-md-offset-3 text-center" ng-controller="MainCtrl">',
                '<img src="https://s3.amazonaws.com/technicalmachine-assets/technical-io/tessel-name.png" style="width: 200px; margin: 10px"/>',
                '<img ng-src="{{imgUrl}}" class="img-thumbnail" style="background-color:#fff;min-width:640px;height:480px"/>',
                '<p class="text-center" style="margin:10px">',
                    '<button ng-disabled="downloading" ng-click="takePicture()" class="btn btn-danger"><i class="glyphicon glyphicon-camera"></i></button>',
                '</p>',
            '</div>',
            '<script>',
                "angular.module('tessel', [])",
                ".controller('MainCtrl', function ($scope){",
                    "$scope.takePicture = function(){",
                        "$scope.downloading=true;",
                        "$scope.imgUrl = 'http://' + location.host + '/picture/' + Math.floor(Math.random()*10000);",
                        "setTimeout(function(){ $scope.downloading = false; $scope.$apply(); }, 10000);",
                    "};",
                 "});",
            '</script>',
        '</body>',
    '</html>'].join('\n');
        res.send(body);
    })

    // Listen on port 8080
    .listen(8080);

// When the router gets an HTTP request at /leds/[NUMBER]
router.get("/leds/{led}", function(req, res) {
  console.log('which led?', req.body.led)
  // Grab the LED being toggled
  var index = req.body.led;
  // Toggle the LED
  tessel.led[index].toggle();
  // Send a response
  res.send(200);
});

router.get("/analog",function(req,res){
  var temperature=0; 
        analogReadPin.analogRead(function(error,value){
            console.log('Sended temperature',value);
            res.send(value);
        });  
});

// Create a websocket server on port 8001
ws.createServer(function (conn) {
  console.log("New connection")
  // When we get a packet from a connection
  conn.on("text", function (str) {

    console.log("Received "+str)
    // Parse it as JSON
    var command = JSON.parse(str);
    // Actually set the LED state
    tessel.led[command.led].output(command.on)
    // Echo it back to confirm
    conn.sendText(JSON.stringify(command));
  });
  // Notify the console when the connection closes
  conn.on("close", function (code, reason) {
      console.log("Connection closed")
  })
}).listen(8081)

console.log('Running Server');