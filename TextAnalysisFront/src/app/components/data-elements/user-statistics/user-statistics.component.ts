import { Component, OnInit, OnDestroy, ViewChild } from '@angular/core';
import { Unsubscribe } from 'redux';
import { NgRedux } from 'ng2-redux';
import { Store } from 'src/app/redux/store';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { UserStatisticsService } from 'src/app/services/ApiConnections/user-statistics.service';
import { baseUrl } from 'src/environments/environment';

@Component({
  selector: 'app-user-statistics',
  templateUrl: './user-statistics.component.html',
  styleUrls: ['./user-statistics.component.css']
})
export class UserStatisticsComponent implements OnInit, OnDestroy {
  
  public mUrl=baseUrl;
  public displayedColumns: string[] = ['userID', 'userFirstName', 'userLastName', 'userNickName', 'userEmail', 'userGender', 'userBirthDate',  'userPicture', 'userRole', 'userRegistrationDate', 'userLoginDate'];
  public dataSource = new MatTableDataSource();
  private unsubscribe:Unsubscribe;

  @ViewChild(MatSort) sort: MatSort;

  constructor(
    private redux:NgRedux<Store>,
    private userStatisticsService:UserStatisticsService) {
      this.dataSource = this.redux.getState().dataUserAnaliticsSource;
      this.dataSource.sort = this.sort;
  }

  public ngOnInit(): void {
    this.userStatisticsService.GetAllUserAnalitics();
    this.dataSource = this.redux.getState().dataUserAnaliticsSource;
    this.dataSource.sort = this.sort;
    this.unsubscribe = this.redux.subscribe(()=>{
      this.dataSource = this.redux.getState().dataUserAnaliticsSource;
      this.dataSource.sort = this.sort;
    });
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  public ngOnDestroy(): void {
    this.unsubscribe();
  }

  public isoFormatDMY(myDate) {  
    let d=new Date(myDate)
    function pad(n) {
      return (n<10? '0' :  '') + n
    }
    let dateForUsee = pad(d.getUTCDate()) + '/' + pad(d.getUTCMonth() + 1) + '/' + d.getUTCFullYear() + " : " + d.getHours() + '/' + d.getMinutes() + '/' + d.getUTCSeconds();
    return dateForUsee;
  }

  public from:Date;
  public to:Date;

  public Search(){
    if(this.from!=null && this.to!=null){
      this.userStatisticsService.GetUserAnaliticsByDates(new Date(this.from), new Date(this.to));
    } else if (this.from!=null){
      this.userStatisticsService.GetUserAnaliticsByStart(new Date(this.from));
    } else if (this.to!=null){
      this.userStatisticsService.GetUserAnaliticsByEnd(new Date(this.to));
    } else if (this.from==null && this.to==null){
      this.userStatisticsService.GetAllUserAnalitics();
    }
  }
}
