using CitizenFX.Core;
using CitizenFX.Core.Native;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpEren.Fivem.Radyo
{
    public class Main : BaseScript
    {
        public bool IsShowing { get; set; }
        public bool IsPlaying { get; set; }
        public int LastVehicle { get; set; }
        public Main()
        {
            IsShowing = false;
            IsPlaying = false;
            RegisterNuiEventHandler("StartPlaying", new Action<IDictionary<string, object>>((data) =>
            {
                IsPlaying = true;
            }));
            RegisterNuiEventHandler("StopPlaying", new Action<IDictionary<string, object>>((data) =>
            {
                IsPlaying = false;
            }));

            EventHandlers["onResourceStop"] += new Action<string>((resourceName) =>{ if (API.GetCurrentResourceName() != resourceName) { return; } Tick -= Main_Tick; });

            Tick += Main_Tick;

        }
        
      
        public async Task Main_Tick()
        {
            if (Game.PlayerPed.IsInVehicle())
            {
                if (LastVehicle == Game.PlayerPed.CurrentVehicle.Handle)
                {
                    if(IsPlaying == false) PlayLast();
                }
                else
                {
                    Reset();
                }


                if (!IsShowing)
                {
                    if (API.IsControlJustPressed(1, 44))
                    {
                        IsShowing = true;
                        Show();
                    }
                }
                else
                {
                    API.HideHudComponentThisFrame(16);
                    API.SetNuiFocusKeepInput(true);

                    if (API.IsControlReleased(1, 44))
                    {
                        IsShowing = false;
                        Hide();
                    }
                }
                LastVehicle = Game.PlayerPed.CurrentVehicle.Handle;
            }
            else
            {
                if (IsPlaying)
                {
                    Stop();
                }
            }
            
        }

        public void Show()
        {
            API.SetNuiFocus(true, true);
            API.SendNuiMessage(JsonConvert.SerializeObject(new
            {
                type = "HUD",
                value="SHOW"
            }));
        }
        public void Stop()
        {
            API.SendNuiMessage(JsonConvert.SerializeObject(new
            {
                type = "RADIO",
                value = "STOP"
            }));
        }
        public void PlayLast()
        {
            API.SendNuiMessage(JsonConvert.SerializeObject(new
            {
                type = "RADIO",
                value = "LAST"
            }));
        }
        public void Reset()
        {
            API.SendNuiMessage(JsonConvert.SerializeObject(new
            {
                type = "RADIO",
                value = "RESET"
            }));
        }
        public void Hide()
        {
            API.SetNuiFocus(true, false);
            API.SendNuiMessage(JsonConvert.SerializeObject(new
            {
                type = "HUD",
                value = "HIDE"
            }));
        }

        public void RegisterNuiEventHandler(string eventName, Action<IDictionary<string, object>> action)
        {
            API.RegisterNuiCallbackType(eventName);
            EventHandlers[$"__cfx_nui:{eventName}"] += new Action<ExpandoObject>(o =>
            {
                IDictionary<string, object> data = o;
                action.Invoke(data);
            });
        }
    }
}
