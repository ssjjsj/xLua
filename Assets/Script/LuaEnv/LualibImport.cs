using System.Runtime.InteropServices;
namespace XLua.LuaDLL
{
    public partial class Lua
    {
        [DllImport("xlua", CallingConvention = CallingConvention.Cdecl)]
        public static extern int luaopen_sproto_core(System.IntPtr L);

        [MonoPInvokeCallback(typeof(LuaDLL.lua_CSFunction))]
        public static int LoadSproto(System.IntPtr L)
        {
            return luaopen_sproto_core(L);
        }

        [DllImport("xlua", CallingConvention = CallingConvention.Cdecl)]
        public static extern int luaopen_lpeg(System.IntPtr L);

        [MonoPInvokeCallback(typeof(LuaDLL.lua_CSFunction))]
        public static int LoadLpeg(System.IntPtr L)
        {
            return luaopen_lpeg(L);
        }

        [DllImport("xlua", CallingConvention = CallingConvention.Cdecl)]
        public static extern int luaopen_ecs_core(System.IntPtr L);

        [MonoPInvokeCallback(typeof(LuaDLL.lua_CSFunction))]
        public static int LoadECS(System.IntPtr L)
        {
            return luaopen_ecs_core(L);
        }
    }
}