/*
 * Tencent is pleased to support the open source community by making xLua available.
 * Copyright (C) 2016 THL A29 Limited, a Tencent company. All rights reserved.
 * Licensed under the MIT License (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the License at
 * http://opensource.org/licenses/MIT
 * Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
*/

using UnityEngine;
using XLua;
using System.Collections.Generic;

namespace XLuaTest
{
    public class Helloworld : MonoBehaviour
    {
        public static List<string> searchPathList = new List<string>()
        {
            "lua",
            "lua/sproto",
        };
        // Use this for initialization
        LuaEnv _luaenv;
        GameSystemManager _gameSystemMgr;
        void Start()
        {
            _gameSystemMgr = gameObject.AddComponent<GameSystemManager>();
            _gameSystemMgr.AddGameSystem<NetworkSystem>();
            initLuaEnv();
        }

        // Update is called once per frame
        void Update()
        {
            
        }

		private void OnDestroy()
		{
            destroyLuaEnv();
        }

        private void initLuaEnv()
		{
            _luaenv = new LuaEnv();
            _luaenv.AddBuildin("sproto.core", XLua.LuaDLL.Lua.LoadSproto);
            _luaenv.AddBuildin("lpeg", XLua.LuaDLL.Lua.LoadLpeg);
            _luaenv.AddBuildin("ecs.core", XLua.LuaDLL.Lua.LoadECS);
            _luaenv.AddLoader((ref string fileName) => {
                foreach (var searchPath in searchPathList)
				{
                    var path = string.Format("{0}/{1}.lua", searchPath, fileName);
                    if (System.IO.File.Exists(path))
					{
                        var bytes = System.IO.File.ReadAllBytes(path);
                        if (bytes != null && bytes.Length != 0)
                        {
                            return bytes;
                        }
                    }
                }
                return null;
            });
            var luaText = System.IO.File.ReadAllText("lua/main.lua");
            _luaenv.DoString(luaText);
        }

        private void destroyLuaEnv()
		{
            _luaenv.Dispose();
        }
	}
}
