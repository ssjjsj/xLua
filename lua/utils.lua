local UTILS = {}

function printTable(t)
    for k,v in pairs(t) do
      if type(v) == "table" then
        print(k .. ":")
        printTable(v)
      else
        print(k .. ": " .. tostring(v))
      end
    end
  end

UTILS.dump = printTable

return UTILS