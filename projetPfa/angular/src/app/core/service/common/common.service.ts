import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class CommonService {
  public base: BehaviorSubject<string> = new BehaviorSubject<string>('');
  public page: BehaviorSubject<string> = new BehaviorSubject<string>('');
  public last: BehaviorSubject<string> = new BehaviorSubject<string>('');
  public lastSub: BehaviorSubject<string> = new BehaviorSubject<string>('');

  public end: BehaviorSubject<string> = new BehaviorSubject<string>('');
  public splitbase: BehaviorSubject<string> = new BehaviorSubject<string>('');
  public splitpage: BehaviorSubject<string> = new BehaviorSubject<string>('');
  public splitlast: BehaviorSubject<string> = new BehaviorSubject<string>('');

  public header: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(
    true
  );
  public mainFooter: BehaviorSubject<boolean> =
    new BehaviorSubject<boolean>(true);
  private _currency = new BehaviorSubject<string>('USD');
  public readonly currency$ = this._currency.asObservable();
}
