using UnityEngine;

namespace Etheral
{
    public class AudioSelector
    {
        CharacterAudio _characterAudio;

        public AudioSelector(CharacterAudio characterAudio)
        {
            _characterAudio = characterAudio;
        }

        public void WeaponHitAudioHandler(AudioImpact typeOfWeapon)
        {
            if (typeOfWeapon == AudioImpact.Null) return;
            _characterAudio.ImpactType = typeOfWeapon;

            if (typeOfWeapon == AudioImpact.Arrow)
                _characterAudio.currentImpact = _characterAudio.AudioLibrary.ArrowDamage;
            if (typeOfWeapon == AudioImpact.Blade)
                _characterAudio.currentImpact = _characterAudio.AudioLibrary.BladeDamage;
            if (typeOfWeapon == AudioImpact.Blunt)
                _characterAudio.currentImpact = _characterAudio.AudioLibrary.BluntDamage;
            if (typeOfWeapon == AudioImpact.Spell)
                return;

            //To remove after testing 12/26/2024
            // _characterAudio.PlayRandomOneShot(_characterAudio.ImpactSource, _characterAudio.currentImpact);
            
            _characterAudio.PlayCurrentImpact();
        }

        public void DetectAndSetFootstep(SurfaceType surfaceType)
        {
            if (_characterAudio.AudioLibrary == null)
            {
                Debug.LogError($"{_characterAudio.gameObject.name} Audio Library is null");
            }

            _characterAudio.CurrentSurface = surfaceType;
            if (_characterAudio.CurrentSurface == SurfaceType.Carpet)
            {
                _characterAudio.currentRunFootsteps = _characterAudio.AudioLibrary.CarpetSurfaceRunning;
                _characterAudio.currentWalkFootsteps = _characterAudio.AudioLibrary.CarpetSurfaceWalking;
            }

            else if (_characterAudio.CurrentSurface == SurfaceType.Grass)
            {
                _characterAudio.currentRunFootsteps = _characterAudio.AudioLibrary.GrassSurfaceRunning;
                _characterAudio.currentWalkFootsteps = _characterAudio.AudioLibrary.GrassSurfaceWalking;
            }

            else if (_characterAudio.CurrentSurface == SurfaceType.Gravel)
            {
                _characterAudio.currentRunFootsteps = _characterAudio.AudioLibrary.GravelSurfaceRunning;
                _characterAudio.currentWalkFootsteps = _characterAudio.AudioLibrary.GravelSurfaceWalking;
            }

            else if (_characterAudio.CurrentSurface == SurfaceType.Hard)
            {
                _characterAudio.currentRunFootsteps = _characterAudio.AudioLibrary.HardSurfaceRunning;
                _characterAudio.currentWalkFootsteps = _characterAudio.AudioLibrary.HardSurfaceWalking;
            }

            else if (_characterAudio.CurrentSurface == SurfaceType.Leaves)
            {
                _characterAudio.currentRunFootsteps = _characterAudio.AudioLibrary.LeavesSurfaceRunning;
                _characterAudio.currentWalkFootsteps = _characterAudio.AudioLibrary.LeavesSurfaceWalking;
            }

            else if (_characterAudio.CurrentSurface == SurfaceType.Metal)
            {
                _characterAudio.currentRunFootsteps = _characterAudio.AudioLibrary.MetalSurfaceRunning;
                _characterAudio.currentWalkFootsteps = _characterAudio.AudioLibrary.MetalSurfaceWalking;
            }

            else if (_characterAudio.CurrentSurface == SurfaceType.Sand)
            {
                _characterAudio.currentRunFootsteps = _characterAudio.AudioLibrary.SandSurfaceRunning;
                _characterAudio.currentWalkFootsteps = _characterAudio.AudioLibrary.SandSurfaceWalking;
            }

            else if (_characterAudio.CurrentSurface == SurfaceType.Snow)
            {
                _characterAudio.currentRunFootsteps = _characterAudio.AudioLibrary.SnowSurfaceRunning;
                _characterAudio.currentWalkFootsteps = _characterAudio.AudioLibrary.SnowSurfaceWalking;
            }

            else if (_characterAudio.CurrentSurface == SurfaceType.Water)
            {
                _characterAudio.currentRunFootsteps = _characterAudio.AudioLibrary.WaterSurfaceRunning;
                _characterAudio.currentWalkFootsteps = _characterAudio.AudioLibrary.WaterSurfaceWalking;
            }

            else if (_characterAudio.CurrentSurface == SurfaceType.Wood)
            {
                _characterAudio.currentRunFootsteps = _characterAudio.AudioLibrary.WoodSurfaceRunning;
                _characterAudio.currentWalkFootsteps = _characterAudio.AudioLibrary.WoodSurfaceWalking;
            }
        }
    }
}