local proto = require "network/proto"
local sproto = require "sproto"

clsBase = require "classic"
local host = sproto.new(proto.s2c):host "package"
local request = host:attach(sproto.new(proto.c2s))

clsNetClient = clsBase:extend()

function clsNetClient:new()
    self.networkSys = CS.NetworkSystem.GetInstance()
    self.session = 0
	self.session2cb = {}
	self.registevent2cb = {}
    self.networkSys.MessageDispatch = function(msg)
        self:recv_package(msg)
    end
end

function clsNetClient:Connect(ip, port)
    self.networkSys:Connect(ip, port)
end

function clsNetClient:SendRequest(name, args, cb)
	self.session = self.session + 1
	self.session2cb[self.session] = cb
	local str = request(name, args, self.session)
	self:send_package(str)
end

function clsNetClient:RegisterNetEvent(name, cb)
	self.registevent2cb[name] = cb
end

function clsNetClient:send_package(package)
	self.networkSys:Send(package)
end

function clsNetClient:recv_package(result)
	--return "RESPONSE", session, nil, header.ud
	local msgType, msgKey, msgData, _ = host:dispatch(result)
	--print(msgType, msgKey, msgData, "recv_package")
	if msgType == "RESPONSE" then
		local cb =  self.session2cb[msgKey]
		if cb then
			cb(msgData)
		end
	else
		local cb = self.registevent2cb[msgKey]
		if cb then
			cb(msgData)
		end
	end
end

return clsNetClient

-- send_request("handshake")
-- send_request("set", { what = "hello", value = "world" })

