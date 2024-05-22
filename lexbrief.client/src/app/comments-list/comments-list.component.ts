import { Component, OnInit } from '@angular/core';
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

  constructor(private service: CommentsListService) {
    this.comments = [];
  }

  ngOnInit(): void {
    this.service.getComments().pipe(
      tap((comments) => {
        this.comments = comments;
      })
    ).subscribe();
  }
}
