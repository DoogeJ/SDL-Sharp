﻿using SDL_Sharp.Ttf;
using SDL_Sharp.Image;
using SDL_Sharp.Mixer;
using SDL_Sharp.Utility;
using SDL_Sharp;
using System;

namespace SDL_PLUS_EXTENSIONS;
class Program
{
    readonly static bool CheckPressed = false;
    static Window window;
    static Renderer renderer;

    static void Main(string[] args)
    {
        //test save data ;)
        {
            string[] myData = new string[] { "unga1", "unga2", "unga3" };

            Utils.SerializeObject(myData, "./unga.html", SerializeType.html);
            Utils.SerializeObject(myData, "./unga.json", SerializeType.json);
            Utils.SerializeObject(myData, "./unga.bin", SerializeType.binary);

            Console.WriteLine(Utils.DeSerializeObject<string[]>("./unga.html", SerializeType.html)[0]);
            Console.WriteLine(Utils.DeSerializeObject<string[]>("./unga.json", SerializeType.json)[1]);
            Console.WriteLine(Utils.DeSerializeObject<string[]>("./unga.bin", SerializeType.binary)[2]);
        }
        //test save data ;)

        WinUtils.SetDpiAwareness(WinHighDpiMode.SystemAware);

        //Init SDL/Image/Mixer/Ttf
        SDL.Init(SdlInitFlags.Video);
        window = SDL.CreateWindow("SDL_EXTENSIONS", SDL.WINDOWPOS_UNDEFINED, SDL.WINDOWPOS_UNDEFINED, 800, 600, WindowFlags.Hidden);
        renderer = SDL.CreateRenderer(window, -1, RendererFlags.Accelerated | RendererFlags.PresentVsync);
        IMG.Init(ImgInitFlags.Jpg);
        TTF.Init();

        int audio_rate = 44100, channels = 2, audio_buffer = 4096;
        ushort audio_format = (ushort)AudioFormatFlags.S16LSB;
        MIX.OpenAudio(audio_rate, audio_format, channels, audio_buffer);
        //Init SDL/Image/Mixer/Ttf

        Sound sound = Sound.CreateSound("./Files/sound.wav");
        sound.Play(1);
        sound.SetVolume(16);

        Texture fegegoso = LoadTexture("./Files/Fedegoso.jpg");

        SDL.ShowWindow(window);
        Event e;
        var running = true;
        while (running)
        {
            Input.Restart();
            while (SDL.PollEvent(out e) == 1)
            {
                Input.Update(e);
                switch (e.Type)
                {
                    case EventType.Quit:
                        running = false;
                        break;
                    case EventType.MouseButtonDown:
                        Console.WriteLine(e.Button.Button);
                        if (e.Button.Button == MouseButton.Left)
                        {
                            Console.WriteLine("pressed");
                        }
                        break;
                }
            }

            SDL.RenderClear(renderer);
            SDL.SetRenderDrawColor(renderer, 135, 206, 235, 255);
            Render(fegegoso);
            //Console.WriteLine(IMG.GetError());

            DrawTextExt("SDL_PLUS_EXTENSIONS", 400, 300, 64, new Color(150, 100, 200, 255), "arial.ttf", true);

            SDL.RenderPresent(renderer);

            if (CheckPressed)
            {
                Console.WriteLine($"Up is pressed: {Input.GetKeyPressed(Keycode.Up)}");
                Console.WriteLine($"Down is pressed: {Input.GetKeyPressed(Keycode.Down)}");
                Console.WriteLine($"Left is pressed: {Input.GetKeyPressed(Keycode.Left)}");
                Console.WriteLine($"Right is pressed: {Input.GetKeyPressed(Keycode.Right)}");
            }
            else
            {
                if (Input.GetKeyDown(Keycode.Space))
                {
                    Console.WriteLine("Key Down Space");
                }
            }
        }

        SDL.DestroyRenderer(renderer);
        SDL.DestroyWindow(window);
        SDL.Quit();
        TTF.Quit();
        IMG.Quit();
    }

    public static void DrawText(string text, int x, int y, int size)
    {
        Font _font = TTF.OpenFont("C:/Windows/Fonts/arial.ttf", size);

        Color color;
        color.R = color.G = color.B = color.A = 0;

        TTF.RenderText_Solid(_font, text, color, out PSurface surfaceMessage);

        Texture Message = SDL.CreateTextureFromSurface(renderer, surfaceMessage);

        int texW;
        int texH;
        SDL.QueryTexture(Message, out _, out _, out texW, out texH);

        Rect dstrect = new Rect(x, y, texW, texH);

        Rect srcrect = new Rect(0, 0, texW, texH);

        SDL.RenderCopy(renderer, Message, ref srcrect, ref dstrect);
        TTF.CloseFont(_font);

        SDL.FreeSurface(surfaceMessage);
        SDL.DestroyTexture(Message);
    }

    public static void DrawTextExt(string text, int x, int y, int size, Color color, string font, bool center)
    {
        Font _font = TTF.OpenFont("C:/Windows/Fonts/" + font, size);
        TTF.RenderText_Solid(_font, text, color, out PSurface surfaceMessage);

        Texture Message = SDL.CreateTextureFromSurface(renderer, surfaceMessage);

        int texW;
        int texH;
        SDL.QueryTexture(Message, out _, out _, out texW, out texH);

        Rect dstrect;

        Rect srcrect = new Rect(0, 0, texW, texH);

        if (!center)
        {
            dstrect = new Rect(x, y, texW, texH);
        }
        else
        {
            dstrect = new Rect(x - (texW / 2), y - (texH / 2), texW, texH);
        }

        SDL.RenderCopy(renderer, Message, ref srcrect, ref dstrect);
        TTF.CloseFont(_font);

        SDL.FreeSurface(surfaceMessage);
        SDL.DestroyTexture(Message);
    }

    internal static Texture LoadTexture(string path)
    {
        Texture texture = IMG.LoadTexture(renderer, path);
        return texture;
    }

    internal static void Render(Texture texture)
    {
        SDL.RenderCopy(renderer, texture, IntPtr.Zero, IntPtr.Zero);
    }
}