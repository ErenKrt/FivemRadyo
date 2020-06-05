local Locals={}
Locals.ped=nil
Locals.Show=false
Locals.isPlaying=false
Locals.LastCar=nil

AddEventHandler("onResourceStart", function (ResName)
    if(GetCurrentResourceName() ~= ResName) then
        return;
      end

      Locals["ped"]=GetPlayerPed(-1)
     
end)

RegisterNUICallback('SetPlaying', function(data, cb)
    Locals.isPlaying=data.isPlaying;
end)

Citizen.CreateThread(function()
    
    repeat
        Locals.ped=GetPlayerPed(-1)
        Citizen.Wait(1000)
    until(Locals.ped~=nil)

    while true do
      Citizen.Wait(0)
      if Locals.ped~=nil then
        if IsPedInAnyVehicle(Locals.ped,false) then

            
            if Locals.isPlaying ~=true then
                SendNUIMessage({
                    type = "StartLast"
                })
                Locals.isPlaying=true
            end
            
            if Locals.LastCar ~= GetPlayersLastVehicle() then
                SendNUIMessage({
                    type = "UpdateCar"
                })
                Locals.LastCar=GetPlayersLastVehicle()
            end

            SetNuiFocusKeepInput(true)
            HideHudComponentThisFrame(16)

            if IsControlJustPressed(1,  44) then
               SetNuiFocus(true,false)
               Locals.Show=true
               SendNUIMessage({
                    type = "HUD",
                    ishow = true
                })
            end
            if IsControlReleased(1,  44) then
               if Locals.Show== true then
                SetNuiFocus(false,false)
                Locals.Show=false
                SendNUIMessage({
                    type = "HUD",
                    ishow = false
                })
               end
             end

        else
            if Locals.isPlaying== true then
                SendNUIMessage({
                    type = "FORCE",
                    stop = true
                })
            end
        end
      end
      --print("Hello world!")
    end
end)