print(_VERSION)

Object = require "classic"
UTILS = require "utils"

clsBase = Object:extend()

function clsBase:new(msg)
    self.msg = msg
end

function clsBase:printmsg()
    print(self.msg)
end

local base = clsBase("hello world form class")
base:printmsg()

local clsNetClient = require "network/netclient"

local netClient = clsNetClient()
netClient:Connect("127.0.0.1", 8888)
netClient:SendRequest("get", { username = "sj"}, function(data)
    UTILS.dump(data)
end)

netClient:RegisterNetEvent("heartbeat", function()
    print("rcv from server heartbeat")
end)

