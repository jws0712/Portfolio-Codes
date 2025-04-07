namespace OTO.Controller
{
    public interface IObserver
    {
        //이벤트 실행
        public void Notify(Subject subject);
    }
}


