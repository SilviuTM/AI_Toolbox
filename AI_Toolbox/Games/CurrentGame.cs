using Microsoft.Xna.Framework.Graphics;

namespace AI_Toolbox.Games
{
    public class CurrentGame
    {
        public bool isAISolving = false;
        public bool isAIReallySolving = false;
        public bool ISAIREALLYSOLVING = false;
        public bool noSolution = false;

        public int waitFrame = 0;
        public int MAX_wait = 60;
        public float waitSlider = 1f;

        public virtual void SolutionButtonClicked()
        {

        }

        public virtual void ReallySolutionButtonClicked()
        {

        }

        public virtual void REALLYSOLUTIONButtonClicked() 
        {
            
        }

        public virtual void ComputeSolution()
        {

        }

        public virtual void LetAISolveIt()
        {

        }

        public virtual void ResetButtonClicked()
        {

        }

        public virtual void RegenButtonClicked()
        {

        }

        public virtual void UpdateUser()
        {

        }

        public virtual void Draw(SpriteBatch sb)
        {

        }
    }
}
