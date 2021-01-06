export interface IUserStatisticsRepository{
    GetAllUserAnalitics(): any;
    GetUserAnaliticsByDates(startDate:Date, endDate:Date): any;
    GetUserAnaliticsByStart(startDate:Date): any;
    GetUserAnaliticsByEnd(endDate:Date): any;
}