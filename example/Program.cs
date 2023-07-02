using System;
using System.IO;
using example;
using ImGuiNET;
using System.Runtime.InteropServices;
using static SDL2.SDL;

public static class Example {

	// Don't forget to copy cimgui.dll and SDL2.dll to the output directory
	
	const int WindowWidth = 1280;
	const int WindowHeight = 720;
	
	// Extra definitions from our custom cimgui binary
	[DllImport("cimgui")]
	static extern bool ImGui_ImplSDL2_InitForSDLRenderer(nint window, nint renderer);
	
	[DllImport("cimgui")]
	static extern bool ImGui_ImplSDLRenderer2_Init(nint renderer);
	
	[DllImport("cimgui")]
	static extern bool ImGui_ImplSDL2_ProcessEvent(in SDL_Event evt);
	
	[DllImport("cimgui")]
	static extern void ImGui_ImplSDLRenderer2_NewFrame();
	
	[DllImport("cimgui")]
	static extern void ImGui_ImplSDL2_NewFrame();
	
	[DllImport("cimgui")]
	static extern void ImGui_ImplSDLRenderer2_RenderDrawData(ImDrawDataPtr ptr);
	
	[DllImport("cimgui")]
	static extern void ImGui_ImplSDLRenderer2_Shutdown();
	
	[DllImport("cimgui")]
	static extern void ImGui_ImplSDL2_Shutdown();

#if ANDROID_LIB
	[UnmanagedCallersOnly(EntryPoint = "cross_example_entrypoint")]
	public static void cross_example_entrypoint()
#else
	public static void Main(string[] args)
#endif
	{
		SDL_Init(SDL_INIT_VIDEO).AssertZero(SDL_GetError);
		
		SDL_SetHint(SDL_HINT_VIDEO_MINIMIZE_ON_FOCUS_LOSS, "0");
		
		var SdlWindow = SDL_CreateWindow("Cimgui + SDL2 cross platform example",
				SDL_WINDOWPOS_UNDEFINED, SDL_WINDOWPOS_UNDEFINED, WindowWidth, WindowHeight,
				SDL_WindowFlags.SDL_WINDOW_ALLOW_HIGHDPI | SDL_WindowFlags.SDL_WINDOW_RESIZABLE)
				.AssertNotNull(SDL_GetError);
		
		// TODO: Other
		SDL_SetHint(SDL_HINT_RENDER_SCALE_QUALITY, "linear");
		
		var SdlRenderer = SDL_CreateRenderer(SdlWindow, -1,
				SDL_RendererFlags.SDL_RENDERER_ACCELERATED |
				SDL_RendererFlags.SDL_RENDERER_PRESENTVSYNC)
				.AssertNotNull(SDL_GetError);
		
		SDL_GetRendererInfo(SdlRenderer, out var info);
		Console.WriteLine($"Initialized SDL with {Marshal.PtrToStringAnsi(info.name)} renderer");
		
		UpdateSize();
		
		var ctx = ImGui.CreateContext();
		
		if (File.Exists("OpenSans-Regular.ttf"))
			ImGui.GetIO().Fonts.AddFontFromFileTTF("OpenSans-Regular.ttf", 20);
		
		ImGui_ImplSDL2_InitForSDLRenderer(SdlWindow, SdlRenderer);
		ImGui_ImplSDLRenderer2_Init(SdlRenderer);
		
		bool IsFullScreen = false;
		
		while (true)
		{
			while (SDL_PollEvent(out var evt) != 0)
			{
				if (evt.type == SDL_EventType.SDL_QUIT || evt.type == SDL_EventType.SDL_WINDOWEVENT &&
					evt.window.windowEvent == SDL_WindowEventID.SDL_WINDOWEVENT_CLOSE ||
					evt.type == SDL_EventType.SDL_KEYDOWN && evt.key.keysym.sym == SDL_Keycode.SDLK_ESCAPE)
				{
					goto break_main_loop;
				}
				else if (evt.type == SDL_EventType.SDL_WINDOWEVENT && evt.window.windowEvent == SDL_WindowEventID.SDL_WINDOWEVENT_RESIZED)
				{
					UpdateSize();
				}
				else if (evt.type == SDL_EventType.SDL_KEYDOWN && evt.key.keysym.sym == SDL_Keycode.SDLK_F11)
				{
					SetFullScreen(!IsFullScreen);
				}
		
				ImGui_ImplSDL2_ProcessEvent(in evt);
			}
		
			ImGui_ImplSDLRenderer2_NewFrame();
			ImGui_ImplSDL2_NewFrame();
			ImGui.NewFrame();
		
			ImGui.ShowDemoWindow();
		
			ImGui.Render();
			SDL_SetRenderDrawColor(SdlRenderer, 0x0, 0x0, 0x0, 0xFF);
			SDL_RenderClear(SdlRenderer);
			ImGui_ImplSDLRenderer2_RenderDrawData(ImGui.GetDrawData());
			SDL_RenderPresent(SdlRenderer);
		}
		break_main_loop:
		
		ImGui_ImplSDLRenderer2_Shutdown();
		ImGui_ImplSDL2_Shutdown();
		ImGui.DestroyContext(ctx);
		
		SDL_DestroyRenderer(SdlRenderer);
		SDL_DestroyWindow(SdlWindow);
		SDL_Quit();
		
		void UpdateSize()
		{   
			SDL_GetWindowSize(SdlWindow, out int w, out int h);
			
			// Scaling workaround for OSX, SDL_WINDOW_ALLOW_HIGHDPI doesn't seem to work
			if (OperatingSystem.IsMacOS())
			{
				SDL_GetRendererOutputSize(SdlRenderer, out int pixelWidth, out int pixelHeight);
				float scaleX = pixelWidth / (float)w;
				float scaleY = pixelHeight / (float)h;
				SDL_RenderSetScale(SdlRenderer, scaleX, scaleY);
			}
		}
		
		void SetFullScreen(bool enableFullScreen)
		{
			IsFullScreen = enableFullScreen;
			SDL_SetWindowFullscreen(SdlWindow, enableFullScreen ? (uint)SDL_WindowFlags.SDL_WINDOW_FULLSCREEN_DESKTOP : 0);
			SDL_ShowCursor(enableFullScreen ? SDL_DISABLE : SDL_ENABLE);
		}
	}
}