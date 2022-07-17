using UnityEngine;
using UnityEngine.Events;

namespace Dev.ComradeVanti.Wurfel
{

    public class TeamBaseKeeper : MonoBehaviour
    {

        public UnityEvent onTurnStarts;
        public UnityEvent<bool> onCanCollectChanged;

        [SerializeField] private Team team;
        [SerializeField] private DiceSpawner diceSpawner;
        [SerializeField] private CameraController cameraController;
        [SerializeField] private DiceLauncher launcher;
        [SerializeField] private ArenaKeeper arenaKeeper;
        [SerializeField] private GameKeeper gameKeeper;
        [SerializeField] private UnityEvent<int> onScoreChanged;

        private int score;
        private readonly DiceBag diceBag = DiceBag.MakeDefault();


        public void OnTurnTeamChanged(Team turnTeam)
        {
            if (turnTeam == team)
                StartTurn();
        }

        private void StartTurn()
        {
            cameraController.LookAt(transform.position + new Vector3(0, 1, 0));
            onTurnStarts.Invoke();

            var canCollect = arenaKeeper.CountDiceOnSide(team) >= 5;
            onCanCollectChanged.Invoke(canCollect);
        }

        public void CollectPoints()
        {
            score += arenaKeeper.CollectDice(team);
            onScoreChanged.Invoke(score);
            this.WaitAndRun(1, () => gameKeeper.SwitchTeam());
        }

        public void SpawnDice()
        {
            var diceName = diceBag.GetRandom();
            var diceGameObject = diceSpawner.SpawnDice(diceName, transform.position);
            launcher.Launch(diceGameObject);
            cameraController.Follow(diceGameObject.transform);
        }

    }

}