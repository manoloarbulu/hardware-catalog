import { StrictMode } from 'react'
import React from 'react'

declare global {
  namespace React {
    interface CSSProperties {
      [key: string]: any;
    }
  }
}
