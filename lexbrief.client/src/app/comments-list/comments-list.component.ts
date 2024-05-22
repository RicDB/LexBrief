import { Component, Input, OnInit } from '@angular/core';
import { CommonModule } from "@angular/common";
import { tap } from 'rxjs';
import { CommentDto, CommentsListService } from './comments-list.service';
import { MaterialModule } from '../material.module';
import { FormsModule, FormControl, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-comments-list',
  providers: [CommentsListService],
  imports: [CommonModule, MaterialModule, FormsModule, ReactiveFormsModule],
  standalone: true,
  templateUrl: './comments-list.component.html',
  styleUrls: ['./comments-list.component.css']
})
export class CommentsListComponent implements OnInit {
  comments: CommentDto[];
  textValue: string;
  inputControl: FormControl = new FormControl('');

  @Input() set refreshComments(value: number) {
    if (value === undefined)
      return;
    this.getComments();
  };

  constructor(private service: CommentsListService) {
    this.comments = [];
    this.textValue = '';
  }

  ngOnInit(): void {
    this.getComments();
  }

  getComments() {
    this.service.getComments().pipe(
      tap((res: CommentDto[]) => {
        this.comments = res;
      })
    ).subscribe();
  }

  addComment() {
    let newItem!: CommentDto;
    newItem = { ...newItem, content: this.textValue }
    return this.service.addComment(newItem).pipe(
      tap((res: CommentDto) => {
        this.comments = [...this.comments, res]
      })
    ).subscribe();
  }

}
