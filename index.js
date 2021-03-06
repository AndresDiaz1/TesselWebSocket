var router = require('tiny-router');
var fs = require('fs');
var tessel = require('tessel');
var analogReadPin = tessel.port.B.pin[7];

router
    .use('static', {path: './static'})
    .use('fs', fs)
    .use('defaultPage','default.html')

    .get('/', function (req, res) {
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
    }).listen(8080);

router.get("/leds/{led}", function (req, res) {
    console.log('which led?', req.body.led)
    var index = req.body.led;
    tessel.led[index].toggle();
    res.send("Toggle led: " + index);
});

router.get("/analog", function (req, res) {
    var temperature = 0;
    analogReadPin.analogRead(function (error, value) {
        console.log('Sended temperature', value);
        res.send("La temperatura es de: "+value);
    });
});

setInterval(startReporting, 3000);

function startReporting(){
    console.log("reportando");
}

console.log('Running Server');