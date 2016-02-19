
var config = require("./config.js");
var grpc = require("grpc");
var grpcPackage = grpc.load(config.protoPath).demo;

function getAccount(call, callback) {
    var accountId = call.request.accountId;
    var account = {
        id: accountId,
        email: "some.email@demo.com",
        characters: [
            {
                id: 1,
                name: "randomName"
            },
            {
                id: 2,
                name: "secondRandomname"
            }
        ]
    };

    callback(null, account);
}