import { Component, Input, OnInit } from '@angular/core';
import { CommonModule } from "@angular/common";
import { tap } from 'rxjs';
import { CommentsListService } from './comments-list.service';
import { MaterialModule } from '../material.module';

@Component({
  selector: 'app-comments-list',
  providers: [CommentsListService],
  imports: [CommonModule, MaterialModule],
  standalone: true,
  templateUrl: './comments-list.component.html',
  styleUrls: ['./comments-list.component.css']
})
export class CommentsListComponent implements OnInit {
  comments: any[];
  @Input() set refreshComments(value: number){
    if(value === undefined)
      return;
    this.getComments();
  };

  constructor(private service: CommentsListService) {
    this.comments = [];
  }

  ngOnInit(): void {
    this.getComments();
  }

  getComments(){
    this.service.getComments().pipe(
      tap((comments) => {
        this.comments = comments;
      })
    ).subscribe();
  }
}
