using _Game.Scripts.Core.Services;
using IKhom.SoundSystem.Runtime.components;

public class SoundService : ISoundService
{
    public SoundManager SoundManager { get; }

    public SoundService(SoundManager soundManager)
    {
        SoundManager = soundManager;
    }
}