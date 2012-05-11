using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace HKAAIERII
{
    public class InputHandler
    {
        //Statis field-/property-par for å gjøre InputHandler tilgjengelig overalt.
        static InputHandler _Instance;
        public static InputHandler Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new InputHandler();

                return _Instance;
            }
        }

        /// <summary>
        /// Privat konstruktør, for at man ikke skal kunne opprette flere instanser av inputhandleren
        /// </summary>
        private InputHandler()
        {

        }

        //Fields og Properties som kun finnes på windows:
#if WINDOWS
        //State-objekter for tastatur
        public KeyboardState KeyboardState { get; private set; }
        public KeyboardState PreviousKeyboardState { get; private set; }


        //tastekonfigurasjon:
        public Keys LeftKey = Keys.Left;
        public Keys RightKey = Keys.Right;
        public Keys UpKey = Keys.Up;
        public Keys DownKey = Keys.Down;

        public Keys ActionKey = Keys.Enter;
        public Keys AbortKey = Keys.Escape;
#endif

        //Muse-apiet finne sbåde for Windows og windows phone, så dette kompileres så lenge det ikek er til XBox 360.
#if !XBOX
        public MouseState MouseState { get; private set; }
        public MouseState PreviousMouseState { get; private set; }
#endif

        //GamePad-apiet finnes til windwos og XBox, så her ekskluderer vi bare windows phone
#if !WINDOWSPHONE
        //State-objekter
        public GamePadState GamePadState { get; private set; }
        public GamePadState PreviousGamePadState { get; private set; }

        //Brukes for å bestemme hvilken gamepad som skal brukes.
        public PlayerIndex PlayerIndex = PlayerIndex.One;

        //Tastekonfigurasjon for gamepad:
        public Buttons LeftButton = Buttons.DPadLeft;
        public Buttons RightButton = Buttons.DPadRight;
        public Buttons UpButton = Buttons.DPadUp;
        public Buttons DownButton = Buttons.DPadDown;

        public Buttons ActionButton = Buttons.A;
        public Buttons AbortButton = Buttons.B;
#endif

        /// <summary>
        /// Oppdaterer states for alle enheter.
        /// </summary>
        public void Update()
        {
#if WINDOWS
            PreviousKeyboardState = KeyboardState;
            KeyboardState = Keyboard.GetState();
#endif

#if !XBOX
            PreviousMouseState = MouseState;
            MouseState = Mouse.GetState();
#endif

#if !WINDOWSPHONE
            PreviousGamePadState = GamePadState;
            GamePadState = GamePad.GetState(PlayerIndex);
#endif
        }

        //Tastaturspesifikk kode:
        #region Keyboard

#if WINDOWS
        /// <summary>
        /// Sjekker om en tast ikke er trykket inn
        /// </summary>
        /// <param name="key">knappen som skal sjekkes</param>
        /// <returns>returnerer true om knappen ikke er trykket</returns>
        public Boolean IsKeyUp(Keys key)
        {
            return KeyboardState.IsKeyUp(key);
        }

        /// <summary>
        /// Sjekker om en knapp er trykket in
        /// </summary>
        /// <param name="key">knappen som skal sjekkes</param>
        /// <returns>returnerer true om knappen er trykket inn</returns>
        public Boolean IsKeyDown(Keys key)
        {
            return KeyboardState.IsKeyDown(key);
        }

        /// <summary>
        /// Sjekker om en knapp er blitt trykket inn siden forrige frame
        /// </summary>
        /// <param name="key">knappen som skal sjekkes</param>
        /// <returns>returnerer true om knappen er blitt trykket inn denne framen</returns>
        public Boolean IsKeyPressed(Keys key)
        {
            return IsKeyDown(key) && PreviousKeyboardState.IsKeyUp(key);
        }

        /// <summary>
        /// Sjekker om en knapp er blitt sluppet ut siden forrige frame
        /// </summary>
        /// <param name="key">knappen som skal sjekkes</param>
        /// <returns>returnerer true om knappen er slupept ut</returns>
        public Boolean IsKeyReleased(Keys key)
        {
            return IsKeyUp(key) && PreviousKeyboardState.IsKeyDown(key);
        }
#endif

        #endregion

        //Gamepadspesifikk kode:
        #region GamePad

#if !WINDOWSPHONE
        /// <summary>
        /// Sjekker om en knapp ikke er trykket inn
        /// </summary>
        /// <param name="button">knappen som skal sjekkes</param>
        /// <returns>returnerer true om knappen ikke er trykket inn</returns>
        public Boolean IsButtonUp(Buttons button)
        {
            return GamePadState.IsButtonUp(button);
        }

        /// <summary>
        /// Sjekker om en knapp er trykket inn
        /// </summary>
        /// <param name="button">knappen som skal sjekkes</param>
        /// <returns>returnerer true dersom knappen er inntrykket</returns>
        public Boolean IsButtonDown(Buttons button)
        {
            return GamePadState.IsButtonDown(button);
        }

        /// <summary>
        /// Sjekker om en knapp er blit trykket inn siden forrige frame
        /// </summary>
        /// <param name="button">knappen som skal kontrolleres</param>
        /// <returns>returnerer true dersom knappen er blitt trykket inn siden forrige frame</returns>
        public Boolean IsButtonPressed(Buttons button)
        {
            return GamePadState.IsButtonDown(button) && PreviousGamePadState.IsButtonUp(button);
        }

        /// <summary>
        /// Sjekker om en knapp er blit sluppet ut siden forrige frame
        /// </summary>
        /// <param name="button">knappen som skal kontrolleres</param>
        /// <returns>returnerer true dersom knappen er blitt sluppet ut siden forrige frame</returns>
        public Boolean IsButtonReleased(Buttons button)
        {
            return GamePadState.IsButtonUp(button) && PreviousGamePadState.IsButtonDown(button);
        }
#endif
        #endregion

        //Metoder som samler APIet for gamepad og tastatur. 
        //Bruker koden lengre opp sammen med fieldene for tastekonfigurasjoner for å gjøre det enkelt å kunne programmere til både tastatur 
        //og gamepad uten å endre koden spesifikt for platform.
        #region Common Methods

        #region Down-Methods
        public Boolean IsUpDown()
        {
#if WINDOWS
            return IsKeyDown(UpKey) || IsButtonDown(UpButton);
#elif XBOX
            return IsButtonDown(UpButton);
#else
            return false;
#endif
        }

        public Boolean IsDownDown()
        {
#if WINDOWS
            return IsKeyDown(DownKey) || IsButtonDown(DownButton);
#elif XBOX
            return IsButtonDown(DownButton);
#else
            return false;
#endif
        }

        public Boolean IsLeftDown()
        {
#if WINDOWS
            return IsKeyDown(LeftKey) || IsButtonDown(LeftButton);
#elif XBOX
            return IsButtonDown(LeftButton);
#else
            return false;
#endif
        }

        public Boolean IsRightDown()
        {
#if WINDOWS
            return IsKeyDown(RightKey) || IsButtonDown(RightButton);
#elif XBOX
            return IsButtonDown(RightButton);
#else
            return false;
#endif
        }

        public Boolean IsActionDown()
        {
#if WINDOWS
            return IsKeyDown(ActionKey) || IsButtonDown(ActionButton);
#elif XBOX
            return IsButtonDown(ActionButton);
#else
            return false;
#endif
        }

        public Boolean IsAbortDown()
        {
#if WINDOWS
            return IsKeyDown(AbortKey) || IsButtonDown(AbortButton);
#elif XBOX
            return IsButtonDown(AbortButton);
#else
            return false;
#endif
        }
        #endregion

        #region Up-Methods
        public Boolean IsUpUp()
        {
#if WINDOWS
            return IsKeyUp(UpKey) || IsButtonUp(UpButton);
#elif XBOX
            return IsButtonUp(UpButton);
#else
            return false;
#endif
        }

        public Boolean IsDownUp()
        {
#if WINDOWS
            return IsKeyUp(DownKey) || IsButtonUp(DownButton);
#elif XBOX
            return IsButtonUp(DownButton);
#else
            return false;
#endif
        }

        public Boolean IsLeftUp()
        {
#if WINDOWS
            return IsKeyUp(LeftKey) || IsButtonUp(LeftButton);
#elif XBOX
            return IsButtonUp(LeftButton);
#else
            return false;
#endif
        }

        public Boolean IsRightUp()
        {
#if WINDOWS
            return IsKeyUp(RightKey) || IsButtonUp(RightButton);
#elif XBOX
            return IsButtonUp(RightButton);
#else
            return false;
#endif
        }

        public Boolean IsActionUp()
        {
#if WINDOWS
            return IsKeyUp(ActionKey) || IsButtonUp(ActionButton);
#elif XBOX
            return IsButtonUp(ActionButton);
#else
            return false;
#endif
        }

        public Boolean IsAbortUp()
        {
#if WINDOWS
            return IsKeyUp(AbortKey) || IsButtonUp(AbortButton);
#elif XBOX
            return IsButtonUp(AbortButton);
#else
            return false;
#endif
        }
        #endregion

        #region Pressed-Methods
        public Boolean IsUpPressed()
        {
#if WINDOWS
            return IsKeyPressed(UpKey) || IsButtonPressed(UpButton);
#elif XBOX
            return IsButtonPressed(UpButton);
#else
            return false;
#endif
        }

        public Boolean IsDownPressed()
        {
#if WINDOWS
            return IsKeyPressed(DownKey) || IsButtonPressed(DownButton);
#elif XBOX
            return IsButtonPressed(DownButton);
#else
            return false;
#endif
        }

        public Boolean IsLeftPressed()
        {
#if WINDOWS
            return IsKeyPressed(LeftKey) || IsButtonPressed(LeftButton);
#elif XBOX
            return IsButtonPressed(LeftButton);
#else
            return false;
#endif
        }

        public Boolean IsRightPressed()
        {
#if WINDOWS
            return IsKeyPressed(RightKey) || IsButtonPressed(RightButton);
#elif XBOX
            return IsButtonPressed(RightButton);
#else
            return false;
#endif
        }

        public Boolean IsActionPressed()
        {
#if WINDOWS
            return IsKeyPressed(ActionKey) || IsButtonPressed(ActionButton);
#elif XBOX
            return IsButtonPressed(ActionButton);
#else
            return false;
#endif
        }

        public Boolean IsAbortPressed()
        {
#if WINDOWS
            return IsKeyPressed(AbortKey) || IsButtonPressed(AbortButton);
#elif XBOX
            return IsButtonPressed(AbortButton);
#else
            return false;
#endif
        }
        #endregion

        #region Released-Methods
        public Boolean IsUpReleased()
        {
#if WINDOWS
            return IsKeyReleased(UpKey) || IsButtonReleased(UpButton);
#elif XBOX
            return IsButtonReleased(UpButton);
#else
            return false;
#endif
        }

        public Boolean IsDownReleased()
        {
#if WINDOWS
            return IsKeyReleased(DownKey) || IsButtonReleased(DownButton);
#elif XBOX
            return IsButtonReleased(DownButton);
#else
            return false;
#endif
        }

        public Boolean IsLeftReleased()
        {
#if WINDOWS
            return IsKeyReleased(LeftKey) || IsButtonReleased(LeftButton);
#elif XBOX
            return IsButtonReleased(LeftButton);
#else
            return false;
#endif
        }

        public Boolean IsRightReleased()
        {
#if WINDOWS
            return IsKeyReleased(RightKey) || IsButtonReleased(RightButton);
#elif XBOX
            return IsButtonReleased(RightButton);
#else
            return false;
#endif
        }

        public Boolean IsActionReleased()
        {
#if WINDOWS
            return IsKeyReleased(ActionKey) || IsButtonReleased(ActionButton);
#elif XBOX
            return IsButtonReleased(ActionButton);
#else
            return false;
#endif
        }

        public Boolean IsAbortReleased()
        {
#if WINDOWS
            return IsKeyReleased(AbortKey) || IsButtonReleased(AbortButton);
#elif XBOX
            return IsButtonReleased(AbortButton);
#else
            return false;
#endif
        }
        #endregion

        #endregion
    }
}
