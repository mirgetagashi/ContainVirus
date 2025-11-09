# Contain Virus - Functional Specification

## Overview
A web application that visualizes and solves the "Contain Virus" algorithm problem. Users can create a grid, mark cells as infected, run an algorithm that calculates the minimum walls needed to contain the virus, and view day-by-day progress of how the virus spreads and gets contained.

---

## Page Structure & Functionality

### 1. Header/Navigation
- **Purpose**: Site navigation
- **Elements**: 
  - Logo/Brand name: "ContainVirus"
  - Navigation links (Home, Privacy)

### 2. Main Content Area

#### Section 1: Grid Dimension Selection
- **Purpose**: User sets up the grid size
- **Functionality**:
  - Two number input fields:
    - "Rows" (default: 4, range: 1-20)
    - "Columns" (default: 7, range: 1-20)
  - "Create Grid" button
- **Behavior**:
  - When "Create Grid" is clicked:
    - Validates inputs (must be 1-20)
    - Creates a grid with specified dimensions
    - Shows Section 2 (Grid Display)
    - Hides any previous results/logs
  - If invalid input: Shows error message

#### Section 2: Grid Display (Hidden initially, shown after grid creation)
- **Purpose**: User marks which cells are infected
- **Functionality**:
  - Displays a grid of cells (rows × columns)
  - Each cell is clickable
  - Cells can be toggled between:
    - Empty (white/empty)
    - Infected (shows virus icon)
  - Two buttons:
    - "Start Algorithm" (disabled until at least one cell is infected)
    - "Clear All" (resets all cells to empty, hides results)
- **Behavior**:
  - Clicking a cell toggles its state (empty ↔ infected)
  - Empty cells: No visual indicator
  - Infected cells: Display a virus icon (spiky teal circle)
  - "Start Algorithm" button:
    - Enabled when grid has at least one infected cell
    - When clicked: Sends grid data to server, processes algorithm
    - Button shows "Processing..." during calculation
    - After completion: Shows Section 3 (Results) and Section 4 (Day-by-Day Logs)
  - "Clear All" button:
    - Resets all cells to empty
    - Hides results and logs
    - Grid remains visible

#### Section 3: Results Display (Hidden initially, shown after algorithm completes)
- **Purpose**: Shows the final result
- **Functionality**:
  - Displays total number of walls needed
  - Shows number of days the algorithm took
  - Success message with summary
- **Behavior**:
  - Appears after algorithm completes successfully
  - Shows: "Total Walls needed: [number]"
  - Shows: "The algorithm completed in [X] day(s). Click on a day below to see the progress."
  - If error occurs: Shows error message instead

#### Section 4: Day-by-Day Progress (Hidden initially, shown after algorithm completes)
- **Purpose**: Visualize the algorithm's step-by-step progress
- **Layout**: Two-column layout
  - Left column (25% width): Days Timeline
  - Right column (75% width): Day Details

##### Left Column: Days Timeline
- **Purpose**: List of all days to navigate between
- **Functionality**:
  - List of clickable day buttons:
    - "Day 0 (Initial)" - Shows initial grid state
    - "Day 1 (X walls)" - Shows Day 1 progress
    - "Day 2 (X walls)" - Shows Day 2 progress
    - etc.
  - Active day is highlighted
- **Behavior**:
  - Clicking a day button:
    - Highlights that day (active state)
    - Updates right column with that day's details
    - Shows grid snapshot for that day
  - Day 0 is selected by default when logs are first displayed

##### Right Column: Day Details
- **Purpose**: Shows detailed information and grid for selected day
- **Functionality**:

  **For Day 0 (Initial State)**:
  - Title: "Day 0 - Initial State"
  - Description: "Initial grid configuration before algorithm starts."
  - Grid visualization showing:
    - Infected cells (with virus icon)
    - Empty cells
  - Legend showing what infected cells look like

  **For Day 1, Day 2, etc.**:
  - Title: "Day [X] - After Operations"
  - Statistics:
    - "Walls built this day: [number]"
    - "Cells quarantined this day: [number]"
    - "Cells newly infected this day: [number]"
    - "Total cells quarantined so far: [number]"
  - Grid visualization showing:
    - **Infected cells**: Active infected cells (virus icon)
    - **Contained/Quarantined cells**: Cells that were quarantined (lock icon, different background)
    - **Newly infected cells**: Cells that became infected on this specific day (virus icon, highlighted background)
    - **Empty cells**: Uninfected cells
  - Legend explaining the different cell states

- **Grid Cell States**:
  1. **Empty**: White/empty cell, no icon
  2. **Infected**: Cell with virus icon (teal spiky circle)
  3. **Contained/Quarantined**: Cell with lock icon, red/pink background, red border
  4. **Newly Infected**: Cell with virus icon, yellow/amber background, yellow border (only visible on the day it was infected)

---

## User Flow

### Flow 1: Basic Usage
1. User opens the page
2. User sees "Step 1: Select Grid Dimensions"
3. User enters rows and columns (or uses defaults)
4. User clicks "Create Grid"
5. Grid appears in "Step 2: Click cells to mark as infected"
6. User clicks cells to mark them as infected (virus icons appear)
7. User clicks "Start Algorithm"
8. Button shows "Processing..." and is disabled
9. After processing:
   - Results section appears showing total walls
   - Day-by-Day Progress section appears
   - Day 0 is automatically selected
10. User can click different days in the timeline to see each day's progress
11. User can click "Clear All" to reset and start over

### Flow 2: Error Handling
- If user enters invalid dimensions (outside 1-20 range):
  - Error alert appears
  - Grid is not created
- If algorithm encounters an error:
  - Error message appears in Results section
  - Day-by-Day Progress section is hidden

### Flow 3: Reset/Clear
- User clicks "Clear All":
  - All cells become empty
  - Results section is hidden
  - Day-by-Day Progress section is hidden
  - Grid remains visible and editable
- User creates new grid:
  - Previous results/logs are cleared
  - New empty grid is created

---

## Data & States

### Grid States
- **0**: Empty cell
- **1**: Infected cell
- **-1**: Contained/Quarantined cell (only in day snapshots)

### Day Log Data (per day)
- Day number
- Number of walls built that day
- List of cells quarantined that day (coordinates)
- List of cells newly infected that day (coordinates)
- Grid snapshot after that day's operations (complete grid state)

### Algorithm Behavior
- Algorithm runs day-by-day simulation:
  - Each day, it identifies virus regions
  - Chooses the region with the largest potential spread
  - Quarantines that region (builds walls around it)
  - Other regions spread to adjacent empty cells
  - Process repeats until no more spread is possible
- Each day's actions are logged:
  - Which cells were quarantined
  - Which cells became newly infected
  - Complete grid state after operations

---

## Interactive Elements

### Buttons
1. **Create Grid**: Creates grid with specified dimensions
2. **Start Algorithm**: Runs the algorithm (disabled until grid has infected cells)
3. **Clear All**: Resets grid and hides results
4. **Day buttons** (in timeline): Switch between different days

### Clickable Elements
- **Grid cells**: Toggle between empty and infected
- **Day buttons**: Select which day to view

### Visual Feedback
- **Hover states**: Cells show hover effect when mouse over
- **Active states**: Selected day button is highlighted
- **Processing state**: "Start Algorithm" button shows "Processing..." and is disabled
- **Disabled states**: Buttons are visually disabled when not usable
- **Animations**: 
  - Virus icons have subtle pulse animation
  - Newly infected cells have highlight animation on their day

---

## Responsive Behavior

### Desktop (default)
- Full layout with all sections visible
- Grid cells: 50px × 50px
- Two-column layout for Day-by-Day section

### Mobile/Tablet (smaller screens)
- Grid cells: 40px × 40px
- Layout may stack vertically
- Day timeline and details may stack

---

## Technical Notes for Designer

### What Needs Visual Design
1. **Overall page layout** (sections, spacing, hierarchy)
2. **Cards/Containers** for each section
3. **Grid appearance** (cell borders, spacing, alignment)
4. **Virus icon design** (spiky teal circle - but designer can create their own version)
5. **Lock icon** for contained cells (or alternative visual indicator)
6. **Button styles** (primary, success, secondary, disabled states)
7. **Input fields** (number inputs for rows/columns)
8. **Day timeline list** (list item styles, active state, hover state)
9. **Statistics display** (how to show the numbers and labels)
10. **Legend/Badges** (for explaining cell states)
11. **Color scheme** for different cell states:
    - Empty cells
    - Infected cells
    - Contained/Quarantined cells
    - Newly infected cells
12. **Alert/Message boxes** (success messages, error messages)
13. **Loading/Processing state** (visual indicator while algorithm runs)

### What is Functional (No Design Needed)
- The actual algorithm logic
- Data processing
- API communication
- Grid generation logic
- Day-by-day calculation

### Key Visual Requirements
- **Grid must be clearly visible** with distinct cells
- **Different cell states must be easily distinguishable** (infected vs contained vs newly infected)
- **Day timeline must be clearly navigable** (users should easily see which day is selected)
- **Statistics should be easy to read** (numbers and labels clearly displayed)
- **Virus icons should be recognizable** (but exact design is up to designer)
- **Contained cells need clear visual indicator** (lock icon or alternative)

---

## Summary for Designer

This is an **interactive visualization tool** where users:
1. Create a customizable grid
2. Mark cells as infected by clicking them
3. Run an algorithm that simulates virus containment
4. View step-by-step progress showing:
   - How the virus spreads each day
   - Which cells get quarantined each day
   - The evolution of the grid over time

The designer should focus on making the **grid visualization clear and intuitive**, with **distinct visual states** for different cell types, and an **easy-to-navigate timeline** for viewing different days of the simulation.

The overall feel should be **scientific/educational** but **user-friendly**, with clear visual feedback for all interactions.

