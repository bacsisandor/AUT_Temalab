var http = require('http');
var fs = require('fs');
var shell = require('shelljs');


var server = http.createServer(function (req, res) {
    if (req.url === '/fileupload') {

        if (req.method === 'POST') {
            var body = '';
            req.on('data', function (data) {
                body += data;
            });
            req.on('end', function () {
                fs.writeFile('to_run.py', 'from gpiozero import *\n' + body, function (err) {
                    if (err) throw err;
                    console.log('Saved!');
                    executeShell(res);
                });
            });
        }
    } else {
        res.writeHead(200, {'Content-Type': 'text/html'});
        res.end('Hello World!');
    }
}).listen(8080);

function executeShell(res) {
    if (shell.exec('python to_run.py').code !== 0){
        res.setHeader('Access-Control-Allow-Origin', '*');
        res.writeHead(200, {'Content-Type': 'text/html'});
        res.end('Saved but something went wrong!');
    } else {
        res.setHeader('Access-Control-Allow-Origin', '*');
        res.writeHead(200, {'Content-Type': 'text/html'});
        res.end('Saved and try to execute!');
    }
}