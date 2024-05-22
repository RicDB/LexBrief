import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { CommonModule } from "@angular/common";
import { MaterialModule } from '../material.module';

@Component({
  selector: 'app-sentiment-overview',
  imports: [CommonModule, MaterialModule],
  standalone: true,
  templateUrl: './sentiment-overview.component.html',
  styleUrls: ['./sentiment-overview.component.css']
})
export class SentimentOverviewComponent implements OnInit {
  @Input() summary!: string;
  constructor() {
  }
  @Input() set commentSentiment(value: any) {
    if (!value) return;
    this.positive = value.positive;
    this.neutral = value.neutral;
    this.negative = value.negative;
  }

  positive!: string;
  negative!: string;
  neutral!: string;

  ngOnInit(): void {

  }

}
