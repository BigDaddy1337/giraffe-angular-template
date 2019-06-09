import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

export interface AngularLinks {
  name: string;
  link: string
}

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  links: AngularLinks[] = [];

  constructor(private http: HttpClient) {}

  ngOnInit() {
    this.http.get<AngularLinks[]>("/api/angular-links").subscribe(res => {
      this.links = res;
    })
  }
}
