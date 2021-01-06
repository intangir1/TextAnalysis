import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class LogService {
  private level: LogLevel = LogLevel.All;
  private logWithDate: boolean = true;

  public log(comment: string, msg: any) {
    console.log(Date.now() + ": " + comment + JSON.stringify(msg));
  }

  public debug(comment: string, msg: any) {
    if (!environment.production){
      console.debug(Date.now() + ": " + comment + JSON.stringify(msg));
    }
  }

  public info(comment: string, msg: any) {
    console.info(Date.now() + ": " + comment + JSON.stringify(msg));
  }

  public warn(comment: string, msg: any) {
    console.warn(Date.now() + ": " + comment + JSON.stringify(msg));
  }

  public error(comment: string, msg: any) {
    console.error(Date.now() + ": " + comment + JSON.stringify(msg));
  }

  public fatal(comment: string, msg: any) {
    console.log(Date.now() + ": " + comment + JSON.stringify(msg));
  }

  public off(comment: string, msg: any) {
    console.log(Date.now() + ": " + comment + JSON.stringify(msg));
  }
}

export enum LogLevel {
  All = 0,
  Debug = 1,
  Info = 2,
  Warn = 3,
  Error = 4,
  Fatal = 5,
  Off = 6
}