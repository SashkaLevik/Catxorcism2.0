namespace GameEnvironment.GameLogic.PlayerSkills
{
    public class DiceReroll : PlayerSkill
    {
        protected override void OnSkillButton()
        {
            _battleHud.ActivateDices();
            _isSkillActive = false;
            //_skillButton.interactable = false;
        }
    }
}