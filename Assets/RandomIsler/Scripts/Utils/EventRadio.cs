using System;

namespace RandomIsleser
{
    public static class EventRadio
    {
        public static event Action<AnimalFlags> OnAnimalCaught;
        public static event Action<AnimalFlags> OnFishCaught;

        public static void AnimalCaught(AnimalFlags animal)
        {
            OnAnimalCaught?.Invoke(animal);
        }

        public static void FishCaught(AnimalFlags animal)
        {
            OnFishCaught?.Invoke(animal);
        }
    }
}
